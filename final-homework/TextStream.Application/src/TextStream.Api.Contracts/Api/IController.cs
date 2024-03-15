using TextStream.Api.Contracts.Requests;
using TextStream.Api.Contracts.Response;
using TextStream.Api.Contracts.Types;

namespace TextStream.Api.Contracts.Api;

/// <summary>
/// Интерфейс Contoller
/// </summary>
public interface IController
{
    /// <summary>
    /// Метод добавления трансляции
    /// </summary>
    /// <param name="model">Модель запроса трансляции</param>
    /// <returns>Номер трансляции</returns>
    public Task<long> AddBroadcast(BroadcastRequest model, CancellationToken cancellationToken);

    /// <summary>
    /// Метод запуска трансляции
    /// </summary>
    /// <param name="Id">Номер трансляции</param>
    public Task StartBroadcast(long Id, CancellationToken cancellationToken);
    /// <summary>
    /// Метод остановки трансляции
    /// </summary>
    /// <param name="Id">Номер трансляции</param>
    public Task StopBroadcast(long Id, CancellationToken cancellationToken);
    /// <summary>
    /// Метод вывода трансляций на указанную дату
    /// </summary>
    /// <param name="date">Дата трансляции</param>
    /// <returns>Список трансляции в указанную дату</returns>
    public Task<List<BroadcastResponse>> ShowGames(DateTime date, CancellationToken cancellationToken);
    /// <summary>
    /// Метод вывода статуса указанной трансляции
    /// </summary>
    /// <param name="id">Номер трансляции</param>
    /// <returns>Статус трансляции</returns>
    public Task<StatusType> ShowStatus(long id, CancellationToken cancellationToken);

    public Task<BroadcastResponse> GetBroadcastById(long id, CancellationToken cancellationToken);
}