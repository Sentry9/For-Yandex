using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Handlers;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.DecisionMakerService;
using Microsoft.Extensions.Logging;

namespace Loans.AppServices.Handlers;

internal class LoanHandler : ILoanHandler
{
    private readonly ILoanValidator _loanValidator;
    private readonly IDecisionMakerService _decisionMakerService;
    private readonly IClientRepository _clientRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly ILogger<LoanHandler> _logger;

    public LoanHandler(ILoanValidator loanValidator, IDecisionMakerService decisionMakerService, IClientRepository clientRepository, ILoanRepository loanRepository, ILogger<LoanHandler> logger)
    {
        _loanValidator = loanValidator;
        _decisionMakerService = decisionMakerService;
        _clientRepository = clientRepository;
        _loanRepository = loanRepository;
        _logger = logger;
    }

    public async Task<long> AddLoan(LoanRequest model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на создание заявки на кредит");

        try
        {
            _loanValidator.Validate(model);

            var client = await _clientRepository.GetClientByIdAsync(model.ClientId, cancellationToken);

            LoanApplicationModel newLoan = new LoanApplicationModel()
            {
                ClientModel = client,
                TermInYears = model.TermInYears,
                Amount = model.DesiredAmount,
                Status = 0,
                CreationDate = model.CreationDate,
                ExpectedInterestRate = 15
            };

            newLoan.Id = await _loanRepository.CreateLoanAsync(newLoan, cancellationToken);

            await SendLoanStatus(newLoan);

            _logger.LogInformation("Создан запрос на отправку кредитной заявки с ID {id} в сервис принятия решений", newLoan.Id);
            _logger.LogInformation("Создана заявка на кредит с ID {id}", newLoan.Id);

            return newLoan.Id;
        }
        catch (LoanException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на создание кредитной заявки.");
            throw;
        }
    }

    public async Task SendLoanStatus(LoanApplicationModel model)
    {
        _logger.LogInformation("Получен запрос на отправку статуса для заявки на кредит с ID {id}", model.Id);

        try
        {
            await _decisionMakerService.CalculateDecision(model, new CancellationToken());
            _logger.LogInformation("Заявка на кредит с ID {id} отправлена в сервис принятия решений", model.Id);
        }
        catch (LoanException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при отправке статуса кредитной заявки в сервис принятия решений.");
            throw;
        }
    }

    public async Task SetLoanStatus(LoanApplicationModel model)
    {
        _logger.LogInformation("Получен запрос на установку статуса для заявки на кредит с ID {id}", model.Id);

        try
        {
            var newLoan = await GetLoanById(model.Id, new CancellationToken());
            newLoan.Status = model.Status;

            await _loanRepository.UpdateLoanAsync(newLoan, CancellationToken.None);

            _logger.LogInformation("Установлен статус для заявки на кредит с ID {id}", model.Id);
        }
        catch (LoanException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на установку статуса кредитной заявки.");
            throw;
        }
    }

    public async Task<LoanApplicationModel> GetLoanById(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на получение заявки на кредит по ID {id}", id);

        try
        {
            var loan = await _loanRepository.GetLoanByIdAsync(id, cancellationToken);

            _logger.LogInformation("Получена заявка на кредит с ID {id}", id);
            return loan;
        }
        catch (LoanException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на получение кредитной заявки по ID.");
            throw;
        }
    }
}