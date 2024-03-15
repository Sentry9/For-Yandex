using TextStream.Api.Contracts.Types;

namespace TextStream.Api.Contracts.Response;
/// <summary>
/// Модель трансляции для ответа
/// </summary>
public class BroadcastResponse
{
    public long id { get; set; }
    public string HomeCommandName { get; set; }
    public string GuestCommandName { get; set; }
    public StatusType StatusType { get; set; }
    public DateTime DateStart { get; set; }
}