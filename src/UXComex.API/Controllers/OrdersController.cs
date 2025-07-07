using System.Net;
using Microsoft.AspNetCore.Mvc;
using UXComex.API.Controllers.Abstract;
using UXComex.Application.Interfaces.Services;
using UXComex.Domain.DTOs.Order;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;

namespace UXComex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderAppService orderAppService) : BaseApiController
    {
        private readonly IOrderAppService _orderAppService = orderAppService;

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OrderResponseDTO))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
            => BaseResponse(await _orderAppService.GetByIdAsync(id));

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedResponse<OrderMinimalResponseDTO>))]
        public async Task<IActionResult> GetPaginatedAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int itemsPerPage = 10,
            [FromQuery] string? search = "",
            [FromQuery] OrderSearchField? field = OrderSearchField.ClientId)
            => BaseResponse(await _orderAppService.GetPaginatedAsync(pageNumber, itemsPerPage, search, field));

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> PostAsync([FromBody] OrderRequestDTO request)
            => BaseResponse(await _orderAppService.CreateAsync(request));

        [HttpPatch("status/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> ChangeStatusAsync(Guid id, [FromBody] OrderStatus status)
            => BaseResponse(await _orderAppService.ChangeOrderStatusAsync(id, status));

        [HttpPost("items/{orderId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> AddNewOrderItemAsync(Guid orderId, [FromBody] OrderItemRequestDTO request)
            => BaseResponse(await _orderAppService.AddNewOrderItemAsync(orderId, request));

        [HttpDelete("items/{itemId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> RemoveOrderItemAsync(Guid itemId)
            => BaseResponse(await _orderAppService.RemoveOrderItemAsync(itemId));
    }
}
