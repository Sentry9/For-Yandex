namespace Loans.Api.Contracts.Requests;

/// <summary>
/// Модель клиента для запроса
/// </summary>
public class ClientRequest
{
    /// <summary>
    /// свойство Имени клиента
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// свойство фамилии клиента
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// свойство отчества клиента
    /// </summary>
    public string? MiddleName { get; set; }
    /// <summary>
    /// свойство даты рождения клиента
    /// </summary>
    public DateTime BirthDate { get; set; }
    /// <summary>
    /// свойство зарплаты клиента
    /// </summary>
    public decimal Salary { get; set; }
}