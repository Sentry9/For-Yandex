namespace Loans.AppServices.Contracts.Exceptions;

public class ServicesException : Exception
{
    public ServicesException(string? message) : base(message)
    {
    }

    public ServicesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}