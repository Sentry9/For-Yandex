using Loans.AppServices.Contracts.Models;

namespace Loans.AppServices.Contracts.RepositoryInterfaces;

public interface ILoanRepository
{
    Task<LoanApplicationModel> GetLoanByIdAsync(long loanId, CancellationToken cancellationToken);
    Task<List<LoanApplicationModel>> GetLoansByClientIdAsync(long clientId, CancellationToken cancellationToken);
    Task<long> CreateLoanAsync(LoanApplicationModel loan, CancellationToken cancellationToken);
    Task UpdateLoanAsync(LoanApplicationModel loan, CancellationToken cancellationToken);
}