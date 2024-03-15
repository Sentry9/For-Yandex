namespace Loans.Api.Contracts.Requests;

/// <summary>
/// Модель клиента для фильтрации
/// </summary>
public class ClientFilterRequest
{
    /// <summary>
    /// свойство Имени клиента
    /// </summary>
    public string? Firstname { get; set; }
    /// <summary>
    /// свойство фамилии клиента
    /// </summary>
    public string? Lastname { get; set; }
    /// <summary>
    /// свойство отчества клиента
    /// </summary>
    public string? Middlename { get; set; }
    /// <summary>
    /// свойство даты рождения клиента
    /// </summary>
    public DateTime? BirthDate { get; set; }
    /// <summary>
    /// свойство зарплаты клиента
    /// </summary>
    public decimal? Salary { get; set; }
}