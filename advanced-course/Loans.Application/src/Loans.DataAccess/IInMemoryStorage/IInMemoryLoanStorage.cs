using Loans.AppServices.Contracts.Models;

namespace Loans.DataAccess.IInMemoryStorage;

public interface IInMemoryLoanStorage
{
    Task<LoanApplicationModel> GetLoanByIdAsync(long clientId, CancellationToken cancellationToken);

    Task<long> CreateLoanAsync(LoanApplicationModel client, CancellationToken cancellationToken);

    Task UpdateLoanAsync(LoanApplicationModel client, CancellationToken cancellationToken);

    Task<List<LoanApplicationModel>> GetLoansByClientIdAsync(long clientId, CancellationToken cancellationToken);
}