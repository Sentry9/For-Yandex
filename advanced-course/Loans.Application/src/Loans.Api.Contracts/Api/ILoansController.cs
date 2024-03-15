using Loans.Api.Contracts.Requests;
using Loans.Api.Contracts.Status;

namespace Loans.Api.Contracts.Api;
/// <summary>
/// Интерфейс LoansController
/// </summary>
public interface ILoansController
{
    /// <summary>
    /// Метод добавления контракта
    /// </summary>
    /// <param name="model"> Модель запроса контракта</param>
    /// <returns>Идентификатор кредита</returns>
    public Task<long> AddLoan(LoanRequest model, CancellationToken cancellationToken);
    /// <summary>
    /// Метод получения статуса кредита
    /// </summary>
    /// <param name="id">Идентификатор кредита</param>
    /// <returns>Статус кредита</returns>
    public Task<LoansStatus> GetLoanStatus(long id, CancellationToken cancellationToken);
}