using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Responses;

namespace Loans.AppServices.Contracts.Handlers;

public interface IClientHandler
{
    public Task<long> AddClient(ClientRequest model, CancellationToken cancellationToken);
    public Task<List<LoanIdResponse>> GetContracts(long clientId, CancellationToken cancellationToken);
    public Task<ClientResponse> GetClientById(long clientId, CancellationToken cancellationToken);
    public Task<List<ClientIdResponse>> GetFilteredClient(string? firstName, string? lastName, string? middlename, DateTime? birthDate, decimal? salary, CancellationToken cancellationToken);
}