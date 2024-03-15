using System.Text;
using Loans.Api.Contracts.Requests;
using Loans.AppServices.Contracts.Exceptions;
using Loans.AppServices.Contracts.Validators;
using Loans.AppServices.Limits;
using Microsoft.Extensions.Options;

namespace Loans.AppServices.Validators;

public class LoanValidator : ILoanValidator
{
    private readonly LoanApplicationLimitsOptions _loanApplicationLimitsOptions;
    public LoanValidator(IOptions<LoanApplicationLimitsOptions> loanConfiguration)
    {
        _loanApplicationLimitsOptions = loanConfiguration.Value;
    }
    public void Validate(LoanRequest model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "Модель заявки на кредит не может быть null.");
        }
        var validationErrors = new List<string>();
        
        if (model.DesiredAmount <= 0)
        {
            validationErrors.Add("Желаемая сумма кредита должна быть положительным числом.");
        }

        if (model.TermInYears <= 0)
        {
            validationErrors.Add("Срок кредита должен быть положительным числом.");
        }
        
        if (model.DesiredAmount < (decimal?)_loanApplicationLimitsOptions.MinLoanAmount)
        {
            validationErrors.Add($"Запрашиваемая сумма кредита слишком мала.");
        }

        if (model.DesiredAmount > (decimal?)_loanApplicationLimitsOptions.MaxLoanAmount)
        {
            validationErrors.Add($"Запрашиваемая сумма кредита слишком велика (максимум: {_loanApplicationLimitsOptions.MaxLoanAmount}).");
        }

        if (model.TermInYears < _loanApplicationLimitsOptions.MinLoanTermInYears)
        {
            validationErrors.Add($"Срок кредита слишком короткий.");
        }

        if (model.TermInYears > _loanApplicationLimitsOptions.MaxLoanTermInYears)
        {
            validationErrors.Add($"Срок кредита слишком длинный (максимум: {_loanApplicationLimitsOptions.MaxLoanTermInYears} лет).");
        }
        

        if (validationErrors.Count > 0)
        {
            var errorsStringBuilder = new StringBuilder();
            foreach (string s in validationErrors)
            {
                errorsStringBuilder.Append(s);
                errorsStringBuilder.Append(' ');
            }
            throw new LoanValidationException(errorsStringBuilder.ToString());
        }
    }
}

