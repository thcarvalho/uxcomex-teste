using UXComex.Domain.Abstract;
using UXComex.Domain.DTOs.Order;
using UXComex.Domain.Enums;
using UXComex.Domain.Validations;

namespace UXComex.Domain.Entities;

public class Order : BaseEntity, IAggregateRoot
{
    public Guid ClientId { get; protected set; }
    public Client Client { get; protected set; }
    public List<OrderItem> OrderItems { get; protected set; } = new();
    public DateTime OrderDate { get; protected set; }
    public decimal TotalAmount { get; protected set; }
    public OrderStatus Status { get; protected set; }

    protected Order() { }

    public Order(Guid clientId)
    {
        ClientId = clientId;
        OrderDate = DateTime.Now;
        Status = OrderStatus.Pending;

        Validate();
    }

    public OrderItem AddOrderItem(Guid productId, int quantity, decimal price)
    {
        var orderItem = new OrderItem(productId, Id, quantity, price);
        OrderItems.Add(orderItem);
        CalculateTotalAmount();
        return orderItem;
    }

    public void ChangeStatus(OrderStatus status)
        => Status = status;

    public void CalculateTotalAmount()
        => TotalAmount = OrderItems.Sum(item => item.Quantity * item.UnitPrice);

    public void RegisterClient(Client client)
        => Client = client;

    public void Validate()
        => base.Validate(new OrderEntityValidator(), this);
}