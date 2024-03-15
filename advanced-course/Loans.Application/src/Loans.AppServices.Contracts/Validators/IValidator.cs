namespace Loans.AppServices.Contracts.Validators;

public interface IValidator<T>
{
    void Validate(T model);
}