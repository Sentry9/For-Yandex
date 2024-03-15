using Loans.AppServices.Contracts.Models;
using Loans.AppServices.Contracts.RepositoryInterfaces;
using Loans.DataAccess.Exceptions;
using Loans.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Loans.DataAccess.Repository;

internal class LoanRepository : ILoanRepository
{
    private readonly DataContext _dataContext;

    public LoanRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<LoanApplicationModel> GetLoanByIdAsync(long loanId, CancellationToken cancellationToken)
    {
        var loan = await _dataContext.Loans
            .Include(l => l.Client)
            .FirstOrDefaultAsync(l => l.Id == loanId, cancellationToken);

        return loan != null
            ? LoanMapper.MapToLoanApplicationModel(loan, loan.Client) 
            : throw new NotFoundException("Клиент или/и кредит не найден");
    }

    public async Task<List<LoanApplicationModel>> GetLoansByClientIdAsync(long clientId, CancellationToken cancellationToken)
    {
        var loans = await _dataContext.Loans
            .Include(l => l.Client)
            .Where(loan => loan.ClientId == clientId)
            .ToListAsync(cancellationToken);

        var loanApplicationModels = loans
            .Select(loan => LoanMapper.MapToLoanApplicationModel(loan, loan.Client))
            .ToList();

        return loanApplicationModels;
    }

    public async Task<long> CreateLoanAsync(LoanApplicationModel loan, CancellationToken cancellationToken)
    {
        var dbLoan = LoanMapper.MapToDbLoanModel(loan);
        _dataContext.Loans.Add(dbLoan);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return dbLoan.Id;
    }

    public async Task UpdateLoanAsync(LoanApplicationModel loan, CancellationToken cancellationToken)
    {
        var existingLoan = await _dataContext.Loans.FindAsync(new object?[] {loan.Id}, cancellationToken);
        
        if (existingLoan != null)
        {
            existingLoan.TermInYears = loan.TermInYears;
            existingLoan.ClientId = loan.ClientModel.Id;
            existingLoan.Status = loan.Status;
            existingLoan.Amount = loan.Amount;
            existingLoan.CreationDate = loan.CreationDate;
            existingLoan.RejectReason = loan.RejectReason;
            existingLoan.ExpectedInterestRate = loan.ExpectedInterestRate;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new NotFoundException("Кредит не найден");
        }
    }
}