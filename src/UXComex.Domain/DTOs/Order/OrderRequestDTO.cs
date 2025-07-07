namespace UXComex.Domain.DTOs.Order;

public record OrderRequestDTO
{
    public Guid ClientId { get; set; }
    public IEnumerable<OrderItemRequestDTO> OrderItems { get; set; }
}