using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.Limits;
using Loans.AppServices.Validators;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Loans.AppServices.Tests.Validators;

public class ClientValidatorTests
{
    private IClientValidator _validator;
    
    public ClientValidatorTests()
    {
        var configuration = Substitute.For<IOptions<ClientLimitsOptions>>();
        configuration.Value.ReturnsForAnyArgs(new ClientLimitsOptions
        {
            MinAge = 18,
            MaxAge = 65,
            MinSalary = 500
        });
        _validator = new ClientValidator(configuration);
    }

    [Fact]
    public void AddClient_InvalidName_ClientValidationException()
    {
        // arrange
        var request = new ClientRequest()
        {
            FirstName = "",
            LastName = "Иванов",
            MiddleName = "Иванович",
            BirthDate = new DateTime(2003, 5, 23),
            Salary = 1000
        };
        
        // act and assert
        Assert.Throws<ClientValidationException>(() => _validator.Validate(request)); // Assert: Проверка результата теста
    }

    [Fact]
    public void AddClient_InvalidSecondName_ClientValidationException()
    {
        // arrange
        var request = new ClientRequest()
        {
            FirstName = "Иван",
            LastName = "",
            MiddleName = "Иванович",
            BirthDate = new DateTime(2003, 5, 23),
            Salary = 1000
        };
        
        // act and assert
        Assert.Throws<ClientValidationException>(() => _validator.Validate(request)); // Assert: Проверка результата теста
    }

    [Fact]
    public void AddClient_InvalidMiddleName_ClientValidationException()
    {
        // arrange
        var request = new ClientRequest()
        {
            FirstName = "Иван",
            LastName = "Иванов",
            MiddleName = "",
            BirthDate = new DateTime(2003, 5, 23),
            Salary = 1000
        };
        
        // act and assert
        Assert.Throws<ClientValidationException>(() => _validator.Validate(request)); // Assert: Проверка результата теста
    }

    [Fact]
    public void AddClient_ValidData_ClientValidationException()
    {
        // arrange
        var request = new ClientRequest()
        {
            FirstName = "Иван",
            LastName = "Иванов",
            MiddleName = "Иванович",
            BirthDate = new DateTime(2003, 5, 23),
            Salary = 1000
        };
        
        // act
        var exception = Record.Exception(() => _validator.Validate(request));
        
        // assert
        Assert.Null(exception); // Assert: Проверка результата теста
    }
}