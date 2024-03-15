namespace Loans.AppServices.Contracts.Exceptions;

public class DecisionMakerUrlException : Exception
{
    public DecisionMakerUrlException(string? message) : base(message)
    {
    }
}