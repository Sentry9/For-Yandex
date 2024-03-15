using TextStream.AppServices.Contracts.Models;

namespace TextStream.AppServices.Contracts.RepositoryInterfaces;

public interface IBroadcastRepository
{
    Task<BroadcastModel?> GetBroadcastByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<BroadcastModel>> GetBroadcastsAsync(DateTime startTime, CancellationToken cancellationToken);
    Task<long> CreateBroadcastAsync(BroadcastModel broadcast, CancellationToken cancellationToken);
    Task UpdateBroadcastAsync(BroadcastModel viewer, CancellationToken cancellationToken);
}