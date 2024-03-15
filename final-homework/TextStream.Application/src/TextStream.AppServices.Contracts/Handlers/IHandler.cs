using TextStream.Api.Contracts.Requests;
using TextStream.Api.Contracts.Response;
using TextStream.Api.Contracts.Types;

namespace TextStream.AppServices.Contracts.Handlers;

public interface IHandler
{
    public Task<long> AddBroadcast(BroadcastRequest model, CancellationToken cancellationToken);
    public Task StartBroadcast(long id, CancellationToken cancellationToken);
    public Task StopBroadcast(long id, CancellationToken cancellationToken);
    public Task<List<BroadcastResponse>> ShowGames(DateTime date, CancellationToken cancellationToken);
    public Task<StatusType> ShowStatus(long id, CancellationToken cancellationToken);
    public Task<BroadcastResponse> GetBroadcastById(long id, CancellationToken cancellationToken);
}