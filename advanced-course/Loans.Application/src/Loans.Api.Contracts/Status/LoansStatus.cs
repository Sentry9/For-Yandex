namespace Loans.Api.Contracts.Status;

public enum LoansStatus : short
{
    Unknown = 0,
    InProgress = 1, /// В ожидании
    Approved = 2, /// Одобрено
    Denied = 3 /// Отклонено
}