using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.DataAccess.Exceptions;
using Loans.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Loans.DataAccess.Repository;

internal class ClientRepository : IClientRepository 
{
    private readonly DataContext _dataContext;

    public ClientRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<ClientModel> GetClientByIdAsync(long clientId, CancellationToken cancellationToken)
    {
        var client = await _dataContext.Clients.FindAsync(new object?[]{clientId}, cancellationToken);
        if (client != null)
        {
            return ClientMapper.MapToClientModel(client);
        }
        return null;
    }

    public async Task<List<ClientModel>> GetClientsAsync(ClientFilterRequest model, CancellationToken cancellationToken)
    {
        var filteredClients = await _dataContext.Clients
            .AsNoTracking()
            .Where(clientEntity =>
                (model.Firstname == null || clientEntity.FirstName.Contains(model.Firstname)) &&
                (model.Lastname == null || clientEntity.LastName.Contains(model.Lastname)) &&
                (model.Middlename == null || clientEntity.MiddleName.Contains(model.Middlename)) &&
                (!model.BirthDate.HasValue || clientEntity.BirthDate.Date == model.BirthDate.Value.Date) &&
                (!model.Salary.HasValue || clientEntity.Salary == model.Salary)
            )
            .ToListAsync(cancellationToken);

        return filteredClients.Select(ClientMapper.MapToClientModel).ToList();
    }

    public async Task<long> CreateClientAsync(ClientModel client, CancellationToken cancellationToken)
    {
        var dbClient = ClientMapper.MapToDbClientModel(client);
        _dataContext.Clients.Add(dbClient);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return dbClient.Id;
    }

    public async Task UpdateClientAsync(ClientModel client, CancellationToken cancellationToken)
    {
        var existingClient = await _dataContext.Clients.FindAsync(new object[] { client.Id }, cancellationToken);
        
        if (existingClient != null)
        {
            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.MiddleName = client.MiddleName;
            existingClient.BirthDate = client.BirthDate;
            existingClient.Salary = client.Salary;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new NotFoundException("Клиент не найден");
        }
    }
}