using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.Limits;
using Loans.AppServices.Validators;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Loans.AppServices.Tests.Validators;

public class LoanValidatorTests
{

    private ILoanValidator _validator;
    
    public LoanValidatorTests()
    {
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
    }

    [Fact]
    public void AddLoan_InvalidArguments_LoanValidationException()
    {
        // arrange
        var request = new LoanRequest()
        {
            ClientId = 1,
            TermInYears = 3,
            DesiredAmount = 5,
            CreationDate = DateTime.Now
        };
        
        // act and assert
        Assert.Throws<LoanValidationException>(() => _validator.Validate(request)); // Assert: Проверка результата теста
    }

    [Fact]
    public void AddLoan_ValidArguments_LoanValidationException()
    {
        // arrange
        var request = new LoanRequest()
        {
            ClientId = 1,
            TermInYears = 3,
            DesiredAmount = 1000,
            CreationDate = DateTime.Now
        };
        
        // act
        var exception = Record.Exception(() => _validator.Validate(request));
        
        // assert
        Assert.Null(exception); // Assert: Проверка результата теста
    }
}