using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Responses;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Handlers;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.AppServices.Contracts.Validators;
using Microsoft.Extensions.Logging;

namespace Loans.AppServices.Handlers;

internal class ClientHandler : IClientHandler
{
    private readonly IClientValidator _clientValidator;
    private readonly IClientRepository _clientRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly ILogger<ClientHandler> _logger;

    public ClientHandler(IClientValidator clientValidator, IClientRepository clientRepository, ILoanRepository loanRepository, ILogger<ClientHandler> logger)
    {
        _logger = logger;
        _clientValidator = clientValidator;
        _clientRepository = clientRepository;
        _loanRepository = loanRepository;
    }

    public async Task<long> AddClient(ClientRequest model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на создание клиента");

        try
        {
            _clientValidator.Validate(model);
            
            ClientModel newClientModel = new ClientModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                BirthDate = model.BirthDate,
                Salary = model.Salary
            };
            
            var id = await _clientRepository.CreateClientAsync(newClientModel, cancellationToken);
            
            _logger.LogInformation("Клиент успешно создан. Идентификатор клиента: {id}", id);
            return id;
        }
        catch (ClientException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на создание клиента.");
            throw;
        }
    }

    public async Task<List<LoanIdResponse>> GetContracts(long clientId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на просмотр кредитов для клиента с идентификатором: {clientId}", clientId);

        try
        {
            List<LoanApplicationModel> contracts = await _loanRepository.GetLoansByClientIdAsync(clientId, cancellationToken);
            
            List<LoanIdResponse> contractsResponses = contracts.Select(contract => new LoanIdResponse
            {
                Id = contract.Id
            }).ToList();
            _logger.LogInformation("Найдены кредиты для клиента с идентификатором {clientId}", clientId);
            return contractsResponses;
        }
        catch (ClientException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на получение кредитов.");
            throw;
        }
    }

    public async Task<ClientResponse> GetClientById(long clientId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на получение информации о клиенте с идентификатором: {clientId}", clientId);

        try
        {
            ClientModel client = await _clientRepository.GetClientByIdAsync(clientId, cancellationToken);

            ClientResponse clientResponse = new ClientResponse()
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate
            };
            
            _logger.LogInformation("Информация о клиенте найдена для клиента с идентификатором: {clientId}", clientId);
            return clientResponse;
        }
        catch (ClientException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на получение информации о клиенте.");
            throw;
        }
    }

    public async Task<List<ClientIdResponse>> GetFilteredClient(string? firstName, string? lastName, string? middlename, DateTime? birthDate, decimal? salary, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на фильтрацию клиентов");

        try
        {
            var model = new ClientFilterRequest()
            {
                Firstname = firstName,
                Lastname = lastName,
                Middlename = middlename,
                BirthDate = birthDate,
                Salary = salary
            };
            
            List<ClientModel> clients = await _clientRepository.GetClientsAsync(model, cancellationToken);
            
            List<ClientIdResponse> clientsResponses = clients.Select(client => new ClientIdResponse
            {
                Id = client.Id
            }).ToList();

            var count = clientsResponses.Count;
            _logger.LogInformation("По фильтру найдено {count} клиентов", count);

            return clientsResponses;
        }
        catch (ClientException ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на фильтрацию клиентов.");
            throw;
        }
    }
}