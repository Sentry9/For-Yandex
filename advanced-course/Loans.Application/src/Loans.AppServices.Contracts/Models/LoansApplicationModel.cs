using Loans.Api.Contracts.Status;

namespace Loans.AppServices.Contracts.Models;

public class LoanApplicationModel
{
    public ClientModel ClientModel { get; set; }
    public long Id { get; set; }
    public int TermInYears { get; set; }
    public decimal Amount { get; set; }
    public LoansStatus Status { get; set; }
    public decimal ExpectedInterestRate { get; set; }
    public DateTime CreationDate { get; set; }
    public string? RejectReason { get; set; }
}