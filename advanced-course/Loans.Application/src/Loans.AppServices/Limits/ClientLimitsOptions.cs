namespace Loans.AppServices.Limits;

public class ClientLimitsOptions
{
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public decimal MinSalary { get; set; }
}