using TextStream.Api.Contracts.Response;
using TextStream.AppServices.Contracts.Models;

namespace TextStream.AppServices.Mapper;

static internal class ToResponse
{
    public static async Task<BroadcastResponse> Map(BroadcastModel model)
    {
        var responseModel = new BroadcastResponse
        {
            id = model.Id,
            GuestCommandName = model.GuestCommandName,
            HomeCommandName = model.HomeCommandName,
            StatusType = model.StatusType,
            DateStart = model.DateStart
        };
        return responseModel;
    }
}