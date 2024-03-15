using TextStream.Api.Contracts.Types;

namespace TextStream.AppServices.Contracts.Models;

public class BroadcastModel
{
    public long Id { get; set; }
    public DateTime DateStart { get; set; }
    public string HomeCommandName { get; set; }
    public string GuestCommandName { get; set; }
    public StatusType StatusType { get; set; }
}