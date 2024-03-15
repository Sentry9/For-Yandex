namespace Loans.AppServices.Contracts.Exceptions;

public class LoanStatusException : ServicesException
{
    public LoanStatusException(string? message) : base(message)
    {
    }
}