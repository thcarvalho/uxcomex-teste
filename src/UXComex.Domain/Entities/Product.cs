using UXComex.Application.Exceptions;
using UXComex.Domain.Abstract;
using UXComex.Domain.Validations;

namespace UXComex.Domain.Entities;

public class Product : BaseEntity, IAggregateRoot
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public decimal Price { get; protected set; }
    public int QuantityInStock { get; protected set; }

    protected Product() { }

    public Product(string name, string description, decimal price, int quantityInStock)
    {
        Name = name;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;

        Validate();
    }

    public void ChangeStockQuantity(int quantity)
    {
        if (quantity <= 0 || quantity > QuantityInStock)
            throw new DomainException("Insufficient stock to reduce.");
        QuantityInStock -= quantity;
    }

    public void Validate()
        => base.Validate(new ProductEntityValidator(), this);
}