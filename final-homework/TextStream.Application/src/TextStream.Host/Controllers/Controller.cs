using Microsoft.AspNetCore.Mvc;
using TextStream.Api.Contracts.Api;
using TextStream.Api.Contracts.Requests;
using TextStream.Api.Contracts.Response;
using TextStream.Api.Contracts.Types;
using TextStream.AppServices.Contracts.Handlers;

namespace TextStream.Host.Controllers
{
    [Route("commentator")]
    [ApiController]
    public class Controller : ControllerBase, IController
    {
        private readonly IHandler _handler;

        public Controller(IHandler handler)
        {
            _handler = handler;
        }
        
        [HttpPost("/broadcast")]
        public async Task<long> AddBroadcast(BroadcastRequest model, CancellationToken cancellationToken)
        {
            long broadcastId = await _handler.AddBroadcast(model, cancellationToken);
            return broadcastId;
        }
        [HttpPost("/Start")]
        public async Task StartBroadcast(long id, CancellationToken cancellationToken)
        {
            await _handler.StartBroadcast(id, cancellationToken);
        }

        [HttpPost("/Stop")]
        public async Task StopBroadcast(long id, CancellationToken cancellationToken)
        {
            await _handler.StopBroadcast(id, cancellationToken);
        }
        [HttpGet("/ShowGames")]
        public async Task<List<BroadcastResponse>> ShowGames(DateTime date, CancellationToken cancellationToken)
        {
            return await _handler.ShowGames(date, cancellationToken);
        }

        [HttpGet("/ShowStatus")]
        public async Task<StatusType> ShowStatus(long id, CancellationToken cancellationToken)
        {
            return await _handler.ShowStatus(id, cancellationToken);
        }
        
        [HttpGet("/id")]
        public async Task<BroadcastResponse> GetBroadcastById(long id, CancellationToken cancellationToken)
        {
            return await _handler.GetBroadcastById(id, cancellationToken);
        }
    }
}
