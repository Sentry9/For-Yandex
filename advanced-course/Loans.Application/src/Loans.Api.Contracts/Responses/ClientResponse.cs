namespace Loans.Api.Contracts.Responses;

/// <summary>
/// Модель клиента для ответа
/// </summary>
public class ClientResponse
{
    /// <summary>
    /// свойство имени клиента
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
}