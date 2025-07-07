using FluentValidation;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Validations;

public class OrderEntityValidator : AbstractValidator<Order>
{
    public OrderEntityValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .NotNull();
    }
}