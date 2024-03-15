namespace TextStream.Api.Contracts.Requests;

/// <summary>
/// Модель трансляции для запроса
/// </summary>
public class BroadcastRequest
{
    public DateTime DateStart { get; set; }
    public string HomeCommandName { get; set; }
    public string GuestCommandName { get; set; }
}