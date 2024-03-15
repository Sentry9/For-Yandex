using Microsoft.EntityFrameworkCore;
using TextStream.AppServices.Contracts.Models;
using TextStream.AppServices.Contracts.RepositoryInterfaces;
using TextStream.DataAccess.Exceptions;
using TextStream.DataAccess.Mappers;

namespace TextStream.DataAccess.Repository;

internal class BroadcastRepository : IBroadcastRepository 
{
    private readonly DataContext _dataContext;

    public BroadcastRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<BroadcastModel> GetBroadcastByIdAsync(long id, CancellationToken cancellationToken)
    {
        var broadcast = await _dataContext.Broadcasts.FindAsync(new object?[]{id}, cancellationToken);
        if (broadcast == null)
        {
            throw new NotFoundException("Не найдена модель трансляции");
        }
        return BroadcastMapper.MapToBroadcastModel(broadcast);
    }

    public async Task<List<BroadcastModel>> GetBroadcastsAsync(DateTime startTime, CancellationToken cancellationToken)
    {
        DateTime startOfDay = startTime.Date;
        DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
        var filteredBroadcasts = await _dataContext.Broadcasts
            .AsNoTracking()
            .Where(broadcastEntity =>
                (broadcastEntity.DateStart >= startOfDay && broadcastEntity.DateStart <= endOfDay)
            )
            .ToListAsync(cancellationToken);

        return filteredBroadcasts.Select(BroadcastMapper.MapToBroadcastModel).ToList();
    }

    public async Task<long> CreateBroadcastAsync(BroadcastModel broadcast, CancellationToken cancellationToken)
    {
        var dbBroadcast = BroadcastMapper.MapToDbBroadcastEntity(broadcast);
        _dataContext.Broadcasts.Add(dbBroadcast);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return dbBroadcast.Id;
    }

    public async Task UpdateBroadcastAsync(BroadcastModel broadcast, CancellationToken cancellationToken)
    {
        var existingBroadcast = await _dataContext.Broadcasts.FindAsync(new object[] { broadcast.Id }, cancellationToken);
        
        if (existingBroadcast != null)
        {
            existingBroadcast.DateStart = broadcast.DateStart;
            existingBroadcast.HomeTeamName = broadcast.HomeCommandName;
            existingBroadcast.GuestTeamName = broadcast.GuestCommandName;
            existingBroadcast.StatusType = broadcast.StatusType;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new NotFoundException("Клиент не найден");
        }
    }
}