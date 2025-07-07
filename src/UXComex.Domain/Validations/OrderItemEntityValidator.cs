using FluentValidation;
using UXComex.Domain.Abstract;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Validations;

public class OrderItemEntityValidator : AbstractValidator<OrderItem>
{
    public OrderItemEntityValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.OrderId)
            .NotNull();

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.UnitPrice)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);
    }
}