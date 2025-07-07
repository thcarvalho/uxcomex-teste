using UXComex.Domain.DTOs.Order;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;

namespace UXComex.Application.Interfaces.Services;

public interface IOrderAppService
{
    Task<PaginatedResponse<OrderMinimalResponseDTO>> GetPaginatedAsync(
        int pageNumber = 1, int itemsPerPage = 10, string? search = "", OrderSearchField? field = null);
    Task<OrderResponseDTO> GetByIdAsync(Guid id);
    Task<OrderResponseDTO> CreateAsync(OrderRequestDTO product);
    Task<OrderResponseDTO> ChangeOrderStatusAsync(Guid id, OrderStatus status);
    Task<OrderResponseDTO> AddNewOrderItemAsync(Guid orderId, OrderItemRequestDTO orderItemRequestDTO);

    Task<bool> RemoveOrderItemAsync(Guid itemId);
}