using System.Diagnostics.Contracts;
using UXComex.Domain.DTOs.Order;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Mappers;

public static class OrderItemMapper
{
    public static OrderItemResponseDTO ToDTO(this OrderItem orderItem)
        => new OrderItemResponseDTO
        {
            Id = orderItem.Id,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product?.Name ?? string.Empty,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice
        };
}