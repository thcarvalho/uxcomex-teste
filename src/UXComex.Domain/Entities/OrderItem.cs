using UXComex.Application.Exceptions;
using UXComex.Domain.Abstract;
using UXComex.Domain.Validations;

namespace UXComex.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid ProductId { get; protected set; }
    public Product Product { get; protected set; }
    public Guid OrderId { get; protected set; }
    public Order Order { get; protected set; }
    public int Quantity { get; protected set; }
    public decimal UnitPrice { get; protected set; }
    
    protected OrderItem() { }

    public OrderItem(Guid productId, Guid orderId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        OrderId = orderId;
        Quantity = quantity;
        UnitPrice = unitPrice;

        Validate();
    }

    public void RegisterProduct(Product product)
        => Product = product;

    public void Validate()
        => base.Validate(new OrderItemEntityValidator(), this);
}