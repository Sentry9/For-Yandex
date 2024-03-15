using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Models;

namespace Loans.AppServices.Contracts.RepositoryInterfaces;

public interface IClientRepository
{
    Task<ClientModel?> GetClientByIdAsync(long clientId, CancellationToken cancellationToken);
    Task<List<ClientModel>> GetClientsAsync(ClientFilterRequest model, CancellationToken cancellationToken);
    Task<long> CreateClientAsync(ClientModel client, CancellationToken cancellationToken);
    Task UpdateClientAsync(ClientModel client, CancellationToken cancellationToken);
}