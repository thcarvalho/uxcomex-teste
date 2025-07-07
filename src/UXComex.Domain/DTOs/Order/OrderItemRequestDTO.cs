namespace UXComex.Domain.DTOs.Order;

public record OrderItemRequestDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    //public decimal UnitPrice { get; set; }
}