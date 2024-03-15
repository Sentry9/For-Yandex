using TextStream.AppServices.Contracts.Exceptions;

namespace TextStream.DataAccess.Exceptions;

public class NotFoundException : ServicesException
{
    public NotFoundException(string? message) : base(message)
    {
    }
}