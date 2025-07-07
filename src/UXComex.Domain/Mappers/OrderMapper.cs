using UXComex.Domain.DTOs.Client;
using UXComex.Domain.DTOs.Order;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Mappers;

public static class OrderMapper
{
    public static Order ToEntity(this OrderRequestDTO orderDto)
        => new Order(orderDto.ClientId);

    public static OrderResponseDTO ToDTO(this Order order)
        => new OrderResponseDTO
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ClientName = order.Client?.Name ?? string.Empty,
            OrderItems = order.OrderItems.Select(item => item.ToDTO()).ToList(),
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status
        };
    public static OrderMinimalResponseDTO ToMinimalDTO(this Order order)
        => new OrderMinimalResponseDTO
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ClientName = order.Client?.Name ?? string.Empty,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status
        };
}