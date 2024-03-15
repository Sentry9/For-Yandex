namespace Loans.AppServices.Contracts.Exceptions;

public class LoanValidationException : ServicesException
{
    public LoanValidationException(string? message) : base(message)
    {
    }
}