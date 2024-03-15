using System.Text;
using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.Limits;
using Microsoft.Extensions.Options;

namespace Loans.AppServices.Validators;

public class ClientValidator : IClientValidator
{
    private readonly ClientLimitsOptions _clientLimitsOptions;
    
    public ClientValidator(IOptions<ClientLimitsOptions> clientConfiguration)
    {
        _clientLimitsOptions = clientConfiguration.Value;
    }
    public void Validate(ClientRequest model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "Модель клиента не может быть null.");
        }
        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(model.FirstName))
        {
            validationErrors.Add("Имя клиента обязательно для заполнения.");
        }

        if (string.IsNullOrWhiteSpace(model.LastName))
        {
            validationErrors.Add("Фамилия клиента обязательна для заполнения.");
        }

        if (string.IsNullOrWhiteSpace(model.MiddleName))
        {
            validationErrors.Add("Отчество клиента обязательно для заполнения.");
        }

        if (CalculateAge(model.BirthDate) < _clientLimitsOptions.MinAge)
        {
            validationErrors.Add($"Клиенту должно быть больше: {_clientLimitsOptions.MinAge} лет.");
        }
        if (model.BirthDate.Kind == DateTimeKind.Utc)
        {
            validationErrors.Add($"Предоставьте дату без временной зоны");
        }
        if (CalculateAge(model.BirthDate) > _clientLimitsOptions.MaxAge)
        {
            validationErrors.Add($"Клиенту должно быть меньше: {_clientLimitsOptions.MaxAge} лет.");
        }

        if (model.Salary <= 0)
        {
            validationErrors.Add("Зарплата должна быть положительным числом.");
        }

        if (model.Salary < (decimal?)_clientLimitsOptions.MinSalary)
        {
            validationErrors.Add($"Зарплата клиента ниже минимальной (минимум: {_clientLimitsOptions.MinSalary}).");
        }

        if (validationErrors.Count > 0)
        {
            var errorsStringBuilder = new StringBuilder();
            foreach (string s in validationErrors)
            {
                errorsStringBuilder.Append(s);
                errorsStringBuilder.Append(' ');
            }
            throw new ClientValidationException(errorsStringBuilder.ToString());
        }
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}
