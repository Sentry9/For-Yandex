using System.Collections.Concurrent;
using Loans.AppServices.Contracts.Models;
using Loans.DataAccess.IInMemoryStorage;

namespace Loans.DataAccess.InMemoryStorage;

internal class InMemoryLoanStorage : IInMemoryLoanStorage
{
    private readonly ConcurrentDictionary<long, LoanApplicationModel> _loans;
    private long _loanSequence = 0;

    public InMemoryLoanStorage()
    {
        _loans = new ConcurrentDictionary<long, LoanApplicationModel>();
    }
    
    public async Task<LoanApplicationModel> GetLoanByIdAsync(long loanId, CancellationToken cancellationToken)
    {
        _loans.TryGetValue(loanId, out var loan);
        return loan;
    }

    public async Task<List<LoanApplicationModel>> GetLoansByClientIdAsync(long clientId, CancellationToken aCancellationToken)
    {
        return new List<LoanApplicationModel>(_loans.Values.Where(loan => loan.ClientModel.Id == clientId));
    }

    public async Task<long> CreateLoanAsync(LoanApplicationModel loan, CancellationToken cancellationToken)
    {
        long loanId = Interlocked.Increment(ref _loanSequence);
        loan.Id = loanId;
        _loans.TryAdd(loanId, loan);
        return loanId;
    }

    public async Task UpdateLoanAsync(LoanApplicationModel loan, CancellationToken cancellationToken)
    {
        _loans.AddOrUpdate(loan.Id, loan, (key, existingLoan) => loan);
    }
}