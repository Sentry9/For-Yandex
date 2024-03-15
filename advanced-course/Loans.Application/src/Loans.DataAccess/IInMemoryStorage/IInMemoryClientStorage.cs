using Loans.AppServices.Contracts.Models;

namespace Loans.DataAccess.IInMemoryStorage;

internal interface IInMemoryClientStorage
{
    IEnumerable<ClientModel> Values { get; }

    Task<ClientModel> GetClientByIdAsync(long clientId, CancellationToken cancellationToken);

    Task<long> CreateClientAsync(ClientModel client, CancellationToken cancellationToken);

    Task UpdateClientAsync(ClientModel client, CancellationToken cancellationToken);
}