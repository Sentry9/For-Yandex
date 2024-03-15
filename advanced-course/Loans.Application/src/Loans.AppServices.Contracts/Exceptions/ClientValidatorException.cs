namespace Loans.AppServices.Contracts.Exceptions;

public class ClientValidationException : ServicesException
{
    public ClientValidationException(string? message) : base(message)
    {
    }
}