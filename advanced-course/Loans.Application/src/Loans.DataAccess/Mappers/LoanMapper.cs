using Loans.Api.Contracts.Status;
using Loans.AppServices.Contracts.Models;
using Loans.DataAccess.Models;

namespace Loans.DataAccess.Mappers;

internal static class LoanMapper
{
    internal static LoanEntity MapToDbLoanModel(LoanApplicationModel model)
    {
        return new LoanEntity
        {
            Id = model.Id,
            ClientId = model.ClientModel.Id,
            TermInYears = model.TermInYears,
            Amount = model.Amount,
            Status = model.Status,
            ExpectedInterestRate = model.ExpectedInterestRate,
            CreationDate = model.CreationDate,
            RejectReason = model.RejectReason
        };
    }

    internal static LoanApplicationModel MapToLoanApplicationModel(LoanEntity entity, ClientEntity clientEntity)
    {
        return new LoanApplicationModel
        {
            Id = entity.Id,
            ClientModel = ClientMapper.MapToClientModel(clientEntity),
            TermInYears = entity.TermInYears,
            Amount = entity.Amount,
            Status = entity.Status,
            ExpectedInterestRate = entity.ExpectedInterestRate,
            CreationDate = entity.CreationDate,
            RejectReason = entity.RejectReason
        };
    }
}