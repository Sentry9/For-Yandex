namespace Loans.AppServices.Contracts.Models;

public class ClientModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Salary { get; set; }
}