using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loans.DataAccess.Models;

internal class ClientEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("first_name")]
    public string FirstName { get; set; }

    [Required]
    [Column("last_name")]
    public string LastName { get; set; }

    [Column("middle_name")]
    public string MiddleName { get; set; }

    [Column("birth_date")]
    public DateTime BirthDate { get; set; }

    [Column("salary")]
    public decimal Salary { get; set; }
}