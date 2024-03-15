using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Models;

namespace Loans.AppServices.Contracts.Handlers;

public interface ILoanHandler
{
    public Task<long> AddLoan(LoanRequest model, CancellationToken cancellationToken);
    public Task<LoanApplicationModel> GetLoanById(long id, CancellationToken cancellationToken);
    public Task SetLoanStatus(LoanApplicationModel model);
    public Task SendLoanStatus(LoanApplicationModel model);
}