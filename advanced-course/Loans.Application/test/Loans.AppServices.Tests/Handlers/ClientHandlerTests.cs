using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Responses;
using Loans.AppServices.Contracts.Handlers;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.Handlers;
using Loans.AppServices.Limits;
using Loans.AppServices.Validators;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using Xunit;

namespace Loans.AppServices.Tests.Handlers;

public class ClientHandlerTests
{
    private readonly IClientHandler _clientHandler;
    
    private IClientValidator _validator;
    
    public ClientHandlerTests()
    {
        var clientRepository = new Mock<IClientRepository>();
        var loanRepository = new Mock<ILoanRepository>();
        var configuration = Substitute.For<IOptions<ClientLimitsOptions>>();
        configuration.Value.ReturnsForAnyArgs(new ClientLimitsOptions
        {
            MinAge = 18,
            MaxAge = 65,
            MinSalary = 500
        });
        _validator = new ClientValidator(configuration);
        _clientHandler = new ClientHandler(_validator, clientRepository.Object, loanRepository.Object);
        
        clientRepository.Setup(repository => repository.CreateClientAsync(It.IsAny<ClientModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientModel clientModel, CancellationToken cancellationToken) => 1);

        loanRepository.Setup(repository => repository.GetLoansByClientIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LoanApplicationModel>());
    }
    
    [Fact]
    public async Task AddClient_ReturnId_ClientHandler()
    {
        // arrange
        var request = new ClientRequest()
        {
            FirstName = "Иван",
            LastName = "Иванов",
            MiddleName = "Иванов",
            BirthDate = new DateTime(2003, 5, 23),
            Salary = 1000
        };
        
        CancellationToken token = new CancellationToken();

        // act
        long actualResult = await _clientHandler.AddClient(request, token);
        
        // assert
        Assert.Equal(1, actualResult); // Assert: Проверка результата теста
    }
    
    [Fact]
    public async Task GetContracts_Null_ClientHandler()
    {
        // arrange
        var request = 1;
        
        CancellationToken token = new CancellationToken();
        
        // act
        List<LoanIdResponse> result = await _clientHandler.GetContracts(request, token);

        // assert
        Assert.Empty(result); // Assert: Проверка результата теста;
    }
}