using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Loans.Api.Contracts.Status;
using Loans.DataAccess.TablesConst;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Loans.DataAccess.Models;

internal class LoanEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("client_id")]
    public long ClientId { get; set; }
    
    [ForeignKey("ClientId")]
    public ClientEntity Client { get; set; }

    [Required]
    [Column("term_in_years")]
    public int TermInYears { get; set; }

    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("status")]
    public LoansStatus Status { get; set; }

    [Column("expected_interest_rate")]
    public decimal ExpectedInterestRate { get; set; }

    [Column("creation_date")]
    public DateTime CreationDate { get; set; }

    [Column("reject_reason")]
    public string? RejectReason { get; set; }
}