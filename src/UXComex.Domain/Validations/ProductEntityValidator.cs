using FluentValidation;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Validations;

public class ProductEntityValidator: AbstractValidator<Product>
{
    public ProductEntityValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2);

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2);

        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.QuantityInStock)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}