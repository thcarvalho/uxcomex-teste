using UXComex.Application.Exceptions;
using UXComex.Application.Interfaces.Services;
using UXComex.Domain.DTOs.Order;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Entities;
using UXComex.Domain.Enums;
using UXComex.Domain.Mappers;
using UXComex.Domain.Repositories;

namespace UXComex.Application.Services;

public class OrderAppService(
    IOrderRepository orderRepository,
    IClientRepository clientRepository,
    IProductRepository productRepository,
    INotificationRepository notificationRepository,
    IProductAppService productAppService) : IOrderAppService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IProductAppService _productAppService = productAppService;

    public async Task<PaginatedResponse<OrderMinimalResponseDTO>> GetPaginatedAsync(
        int pageNumber = 1,
        int itemsPerPage = 10,
        string? search = "",
        OrderSearchField? field = null)
    {
        var response = await _orderRepository.GetPaginatedAsync(
            pageNumber, itemsPerPage, search, Enum.GetName(typeof(OrderSearchField), field));
        
        var count = await _orderRepository.CountAsync(search, Enum.GetName(typeof(OrderSearchField), field));

        return new PaginatedResponse<OrderMinimalResponseDTO>
        {
            Items = response.Select(x => x.ToMinimalDTO()),
            ItemsPerPage = itemsPerPage,
            Page = pageNumber,
            TotalPages = (int)Math.Ceiling((double)count / itemsPerPage),
        };
    }

    public async Task<OrderResponseDTO> GetByIdAsync(Guid id)
    {
        var response = await _orderRepository.GetByIdAsync(id);
        if (response is null)
            throw new NotFoundException($"Order with ID {id} not found.");
        return response.ToDTO();
    }

    public async Task<OrderResponseDTO> CreateAsync(OrderRequestDTO order)
    {
        var entity = order.ToEntity();
        if (entity.IsInvalid()) 
            throw new ValidationException(entity.ErrorsToString());

        if (await ExistClientAsync(entity.ClientId) is false) 
            throw new NotFoundException($"Client with ID {entity.ClientId} not found.");

        var result = await _orderRepository.CreateAsync(entity);

        foreach (var orderItem in order.OrderItems)
        {
            try
            {
                await AddNewOrderItemAsync(result.Id, orderItem);
            }
            catch
            {
                await _orderRepository.DeleteAsync(result.Id);
                throw new ValidationException("Failed to add order items. Order creation rolled back.");
            }
        }
        
        return result.ToDTO();
    }

    public async Task<OrderResponseDTO> ChangeOrderStatusAsync(Guid id, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            throw new NotFoundException($"Order with ID {id} not found.");

        var currentStatus = Enum.GetName(typeof(OrderStatus), order.Status);
        var newStatus = Enum.GetName(typeof(OrderStatus), status);

        order.ChangeStatus(status);

        var response = await _orderRepository.UpdateAsync(order, id);

        await _notificationRepository.CreateAsync(
            new Notification($"Order: {id} - STATUS CHANGE: from '{currentStatus}' to '{newStatus}'"));

        return response.ToDTO();
    }

    public async Task<OrderResponseDTO> AddNewOrderItemAsync(Guid orderId, OrderItemRequestDTO dto)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order is null)
            throw new NotFoundException($"Order with ID {orderId} not found.");

        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product is null)
            throw new NotFoundException($"Product with ID {dto.ProductId} not found.");

        var orderItem = order.AddOrderItem(dto.ProductId, dto.Quantity, product.Price);
        await _productAppService.ChangeProductStockAsync(dto.ProductId, dto.Quantity);

        var response = await _orderRepository.UpdateAsync(order, orderId);
        await _orderRepository.AddNewOrderItemAsync(orderItem);

        return response.ToDTO();
    }

    public async Task<bool> RemoveOrderItemAsync(Guid itemId)
    {
        var orderItem = await _orderRepository.GetOrderItemByIdAsync(itemId);
        if (orderItem is null)
            throw new NotFoundException($"Order Item with ID {itemId} not found.");

        await _orderRepository.RemoveOrderItemAsync(itemId);
        var order = await _orderRepository.GetByIdAsync(orderItem.OrderId);
        order.CalculateTotalAmount();
        var result = await _orderRepository.UpdateAsync(order, orderItem.OrderId);
        
        return result is not null;
    }

    private async Task<bool> ExistClientAsync(Guid clientId)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);
        return client is not null;
    }
}