using Loans.AppServices.Contracts.Exceptions;

namespace Loans.DataAccess.Exceptions;

public class NotFoundException : ServicesException
{
    public NotFoundException(string? message) : base(message)
    {
    }
}