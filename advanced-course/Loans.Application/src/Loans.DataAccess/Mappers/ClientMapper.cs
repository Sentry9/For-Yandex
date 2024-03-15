using Loans.AppServices.Contracts.Models;
using Loans.DataAccess.Models;

namespace Loans.DataAccess.Mappers;

internal static class ClientMapper
{
    public static ClientModel MapToClientModel(ClientEntity entity)
    {
        return new ClientModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            MiddleName = entity.MiddleName,
            BirthDate = entity.BirthDate,
            Salary = entity.Salary
        };
    }

    public static ClientEntity MapToDbClientModel(ClientModel model)
    {
        return new ClientEntity
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MiddleName = model.MiddleName,
            BirthDate = model.BirthDate,
            Salary = model.Salary
        };
    }
}