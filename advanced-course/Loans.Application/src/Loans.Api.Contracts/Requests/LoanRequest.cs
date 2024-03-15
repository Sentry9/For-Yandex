namespace Loans.Api.Contracts.Requests;

/// <summary>
/// Модель контракта для запроса
/// </summary>
public class LoanRequest
{
    /// <summary>
    /// Свойство идентификатора кредита
    /// </summary>
    public long ClientId { get; set; }
    /// <summary>
    /// Свойство срока кредита в годах
    /// </summary>
    public int TermInYears { get; set; }
    /// <summary>
    /// Свойство суммы кредита
    /// </summary>
    public decimal DesiredAmount { get; set; }
    /// <summary>
    /// Свойство даты создания кредита
    /// </summary>
    public DateTime CreationDate { get; set; }
}