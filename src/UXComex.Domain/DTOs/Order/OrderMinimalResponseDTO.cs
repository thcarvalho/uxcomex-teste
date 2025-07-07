using UXComex.Domain.Enums;

namespace UXComex.Domain.DTOs.Order;

public record OrderMinimalResponseDTO
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
}