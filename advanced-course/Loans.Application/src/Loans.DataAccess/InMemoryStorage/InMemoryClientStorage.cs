using System.Collections.Concurrent;
using Loans.AppServices.Contracts.Models;
using Loans.DataAccess.IInMemoryStorage;

namespace Loans.DataAccess.InMemoryStorage;

internal class InMemoryClientStorage : IInMemoryClientStorage
{
    private readonly ConcurrentDictionary<long, ClientModel> _clients;
    private long _clientSequence = 0;
    
    public IEnumerable<ClientModel> Values => _clients.Values;

    public InMemoryClientStorage()
    {
        _clients = new ConcurrentDictionary<long, ClientModel>();
    }

    public async Task<ClientModel> GetClientByIdAsync(long clientId, CancellationToken cancellationToken)
    {
        _clients.TryGetValue(clientId, out var client);
        return client;
    }
    
    public async Task<long> CreateClientAsync(ClientModel client, CancellationToken cancellationToken)
    {
        long clientId = Interlocked.Increment(ref _clientSequence);
        client.Id = clientId;
        _clients.TryAdd(clientId, client);
        return clientId;
    }

    public async Task UpdateClientAsync(ClientModel client, CancellationToken cancellationToken)
    {
        _clients.AddOrUpdate(client.Id, client, (key, existingClient) => client);
    }
}