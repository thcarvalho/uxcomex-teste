using FluentValidation;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Validations;

public class ClientEntityValidator : AbstractValidator<Client>
{
    public ClientEntityValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2);

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .Length(11);
    }    
}