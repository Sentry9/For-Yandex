using Loans.Api.Contracts.Api;
using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Status;
using Loans.AppServices.Contracts.Handlers;
using Loans.AppServices.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loans.Host.Controllers
{
    [Route("loans")]
    [ApiController]
    public class LoansController : ILoansController
    {
        private readonly ILoanHandler _loanHandler;

        public LoansController(ILoanHandler loanHandler)
        {
            _loanHandler = loanHandler;
        }

        [HttpPost()]
        public async Task<long> AddLoan(LoanRequest model, CancellationToken cancellationToken)
        {
            long result = await _loanHandler.AddLoan(model, cancellationToken);
            return result;
        }

        [HttpGet("{loanId}/status")]
        public async Task<LoansStatus> GetLoanStatus(long id, CancellationToken cancellationToken)
        {
            var loan = await _loanHandler.GetLoanById(id, cancellationToken);
            return loan.Status;
        }
        [HttpGet("{loanId}")]
        public async Task<LoanApplicationModel> GetLoan(long loanId, CancellationToken cancellationToken)
        {
            LoanApplicationModel loan = await _loanHandler.GetLoanById(loanId, cancellationToken);
            return loan;
        }
    }
}