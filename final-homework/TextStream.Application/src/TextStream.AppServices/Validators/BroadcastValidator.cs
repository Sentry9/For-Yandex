using System.Text;
using TextStream.Api.Contracts.Requests;
using TextStream.AppServices.Contracts.Exceptions;
using TextStream.AppServices.Contracts.Validators;

namespace TextStream.AppServices.Validators;

public class BroadcastValidator : IBroadcastValidator
{
    public void Validate(BroadcastRequest model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "Модель трансляции не может быть null.");
        }
        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(model.GuestCommandName))
        {
            validationErrors.Add("Название команды, которая играет в гостях, не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(model.HomeCommandName))
        {
            validationErrors.Add("Название команды, которая играет дома, не может быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(model.DateStart.ToString()))
        {
            validationErrors.Add("Время начала игры не должно быть пустым");
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
}
