namespace Loans.AppServices.Contracts.Exceptions;

public class LoanException : ServicesException
{
    public LoanException(string? message) : base(message)
    {
    }
}