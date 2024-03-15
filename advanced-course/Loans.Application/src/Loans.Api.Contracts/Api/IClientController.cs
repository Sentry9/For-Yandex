using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Responses;

namespace Loans.Api.Contracts.Api;

/// <summary>
/// Интерфейс ClientContoller
/// </summary>
public interface IClientController
{
    /// <summary>
    /// Метод добавления клиента
    /// </summary>
    /// <param name="model">Модель запрооса клиента</param>
    /// <returns>Идентификатор клиента</returns>
    public Task<long> AddClient(ClientRequest model, CancellationToken cancellationToken);

    /// <summary>
    /// Метод получения контрактов
    /// </summary>
    /// <param name="clientId">Идентификатор клиента</param>
    /// <returns>список идентификаторов контрактов</returns>
    public Task<List<LoanIdResponse>> GetContracts(long clientId, CancellationToken cancellationToken);
}