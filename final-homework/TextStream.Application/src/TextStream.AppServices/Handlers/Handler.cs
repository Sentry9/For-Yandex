using Microsoft.Extensions.Logging;
using TextStream.Api.Contracts.Requests;
using TextStream.Api.Contracts.Response;
using TextStream.Api.Contracts.Types;
using TextStream.AppServices.Contracts.Exceptions;
using TextStream.AppServices.Contracts.Handlers;
using TextStream.AppServices.Contracts.Models;
using TextStream.AppServices.Contracts.RepositoryInterfaces;
using TextStream.AppServices.Contracts.Validators;
using TextStream.AppServices.Mapper;

namespace TextStream.AppServices.Handlers;

internal class Handler : IHandler
{
    private readonly IBroadcastValidator _broadcastValidator;
    private readonly IBroadcastRepository _broadcastRepository;

    public Handler(IBroadcastValidator broadcastValidator, IBroadcastRepository broadcastRepository, ILogger<Handler> logger)
    {
        _broadcastValidator = broadcastValidator;
        _broadcastRepository = broadcastRepository;
    }

    public async Task<long> AddBroadcast(BroadcastRequest model, CancellationToken cancellationToken)
    {
        try
        {
            _broadcastValidator.Validate(model);
            
            BroadcastModel newBroadcastModel = new BroadcastModel
            {
                DateStart = model.DateStart,
                StatusType = StatusType.Soon,
                HomeCommandName = model.HomeCommandName,
                GuestCommandName = model.GuestCommandName
            };
            
            var id = await _broadcastRepository.CreateBroadcastAsync(newBroadcastModel, cancellationToken);
            return id;
        }
        catch
        {
            throw new BroadcastException("Не удалось создать трансляцию");
        }
    }
    
    public async Task<List<BroadcastResponse>> ShowGames(DateTime date, CancellationToken cancellationToken)
    {
        try
        {
            var models = await _broadcastRepository.GetBroadcastsAsync(date, cancellationToken);
            var responses = new List<BroadcastResponse>();
            foreach (var model in models)
            {
                BroadcastResponse response = await ToResponse.Map(model);
                responses.Add(response);
            }
            return responses;
        }
        catch
        {
            throw new BroadcastException("Произошла ошибка при обработке запроса на просмотр матчей на указанную дату");
        }
    }

    public async Task<BroadcastResponse> GetBroadcastById(long id, CancellationToken cancellationToken)
    {
        var model = await _broadcastRepository.GetBroadcastByIdAsync(id, cancellationToken);
        return await ToResponse.Map(model);
    }
    
    public async Task StartBroadcast(long id, CancellationToken cancellationToken)
    {
        try
        {
            BroadcastModel broadcast = await _broadcastRepository.GetBroadcastByIdAsync(id, cancellationToken);
            broadcast.StatusType = StatusType.InLive;
            await _broadcastRepository.UpdateBroadcastAsync(broadcast, new CancellationToken());
        }
        catch (BroadcastException ex)
        {
            throw new BroadcastException("Произошла ошибка при обработке запроса на запуск трансляции.");
        }
    }

    public async Task<StatusType> ShowStatus(long id, CancellationToken cancellationToken)
    {
        try
        {
            BroadcastModel broadcast = await _broadcastRepository.GetBroadcastByIdAsync(id, cancellationToken);
            return broadcast.StatusType;
        }
        catch
        {
            throw new BroadcastException("Не удалось получить статус трансляции");
        }
    }

    public async Task StopBroadcast(long id, CancellationToken cancellationToken)
    {
        try
        {
            BroadcastModel broadcast = await _broadcastRepository.GetBroadcastByIdAsync(id, cancellationToken);
            broadcast.StatusType = StatusType.Finished;
            await _broadcastRepository.UpdateBroadcastAsync(broadcast, new CancellationToken());
        }
        catch (BroadcastException ex)
        {
            throw new BroadcastException("Произошла ошибка при обработке запроса на остановку трансляции.");
        }
    }
}