using TextStream.AppServices.Contracts.Models;
using TextStream.DataAccess.Models;

namespace TextStream.DataAccess.Mappers;

internal static class BroadcastMapper
{
    public static BroadcastModel MapToBroadcastModel(BroadcastEntity entity)
    {
        return new BroadcastModel
        {
            Id = entity.Id,
            DateStart = entity.DateStart,
            HomeCommandName = entity.HomeTeamName,
            GuestCommandName = entity.GuestTeamName,
            StatusType = entity.StatusType
        };
    }

    public static BroadcastEntity MapToDbBroadcastEntity(BroadcastModel model)
    {
        return new BroadcastEntity
        {
            Id = model.Id,
            DateStart = model.DateStart,
            HomeTeamName = model.HomeCommandName,
            GuestTeamName = model.GuestCommandName,
            StatusType = model.StatusType,
        };
    }
}