namespace TextStream.AppServices.Contracts.Exceptions;

public class BroadcastException : ServicesException
{
    public BroadcastException(string? message) : base(message)
    {
    }
}