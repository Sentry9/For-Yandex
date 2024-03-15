namespace Loans.AppServices.Contracts.Exceptions;

public class ClientException : ServicesException
{
    public ClientException(string? message) : base(message)
    {
    }
}