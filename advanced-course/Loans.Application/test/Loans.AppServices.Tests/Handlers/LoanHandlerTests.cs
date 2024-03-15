using Loans.Api.Contracts.Status;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.DecisionMakerService;
using Loans.AppServices.Handlers;
using Loans.AppServices.Limits;
using Loans.AppServices.Validators;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using Xunit;

namespace Loans.AppServices.Tests.Handlers;

public class LoanHandlerTests
{
    private readonly LoanHandler _loanHandler;
    private readonly ClientHandler _clientHandler;
    private readonly IDecisionMakerService _decisionMakerService;


    private ILoanValidator _validator;
    
    public LoanHandlerTests()
    {
        var clientRepository = new Mock<IClientRepository>();
        var loanRepository = new Mock<ILoanRepository>();
        var configuration = Substitute.For<IOptions<LoanApplicationLimitsOptions>>();
        configuration.Value.ReturnsForAnyArgs(new LoanApplicationLimitsOptions
        {
            MinLoanAmount = 1000,
            MaxLoanAmount = 10000,
            MinLoanTermInYears = 1,
            MaxLoanTermInYears = 10,
            MinSalary = 500
        });
        _validator = new LoanValidator(configuration);
        _decisionMakerService = Substitute.For<IDecisionMakerService>();
        _decisionMakerService.CalculateDecision(null, default).ReturnsForAnyArgs(new CalculateDecisionResponse(12, 12,
            new Decision { DecisionStatus = DecisionStatus.Approval, LoanOffer = new LoanOffer { InterestRate = 15 } }));
        _loanHandler = new LoanHandler(_validator, _decisionMakerService, clientRepository.Object, loanRepository.Object);
    }
    
    [Fact]
    public void SetStatus_Approved()
    {
        // arrange
        var request = new LoanApplicationModel();
        LoansStatus result = LoansStatus.Approved;
        
        // act
        _loanHandler.SetLoanStatus(request, DecisionStatus.Approval);
        
        // assert
        Assert.Equal(request.Status, result);
    }
    
    [Fact]
    public void SetStatus_Denied()
    {
        // arrange
        var request = new LoanApplicationModel();
        LoansStatus result = LoansStatus.Denied;
        
        // act
        _loanHandler.SetLoanStatus(request, DecisionStatus.Refuse);
        
        // Assert
        Assert.Equal(request.Status, result);
    }

    [Fact]
    public void SetStatus_Underwriting()
    {
        // arrange
        var request = new LoanApplicationModel();
        LoansStatus result = LoansStatus.Denied;
        
        // act
        _loanHandler.SetLoanStatus(request, DecisionStatus.Underwriting);
        
        // Assert
        Assert.Equal(request.Status, result);
    }

    [Fact]
    public void SetStatus_InProgress()
    {
        // arrange
        var request = new LoanApplicationModel();
        LoansStatus result = LoansStatus.Denied;
        
        // act
        _loanHandler.SetLoanStatus(request, DecisionStatus.Unknown);
        
        // Assert
        Assert.Equal(request.Status, result);
    }
    
    [Fact] public void SetStatus_ValidArguments_LoanHandler()
    {
        // arrange
        var request = new LoanApplicationModel();
        
        // act
        var exception = Record.Exception(() => _loanHandler.SetLoanStatus(request, DecisionStatus.Refuse));
        
        // assert
        Assert.Null(exception); // Assert: Проверка результата теста
        
    }

    [Fact]
    public void SetStatus_InvalidArguments_LoanHandler()
    {
        // act and assert
        Assert.Throws<LoanStatusException>(() => _loanHandler.SetLoanStatus(null, DecisionStatus.Refuse));
    }
}