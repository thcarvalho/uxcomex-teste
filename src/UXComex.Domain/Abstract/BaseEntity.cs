using FluentValidation.Results;
using FluentValidation;

namespace UXComex.Domain.Abstract;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    private List<string> Errors { get; } = new();

    public string ErrorsToString()
        => Errors.Aggregate("", (current, error) => current + (error + " "));

    public bool IsInvalid()
        => Errors.Count > 0;

    protected void Validate<TValidator, TEntity>(TValidator validator, TEntity obj)
        where TValidator : AbstractValidator<TEntity>
    {
        ClearErrors();
        var validation = validator.Validate(obj);

        if (validation.Errors.Count > 0)
            AddErrorList(validation.Errors);
    }

    private void AddErrorList(List<ValidationFailure> errors)
    {
        foreach (var error in errors)
            Errors.Add(error.ErrorMessage);
    }

    private void ClearErrors()
        => Errors.Clear();
}