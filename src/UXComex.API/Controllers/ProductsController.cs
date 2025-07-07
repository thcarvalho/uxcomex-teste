using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UXComex.API.Controllers.Abstract;
using UXComex.Application.Interfaces.Services;
using UXComex.Domain.DTOs.Product;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;

namespace UXComex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductAppService productAppService) : BaseApiController
    {
        private readonly IProductAppService _productAppService = productAppService;

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> PostAsync([FromBody] ProductRequestDTO request)
            => BaseResponse(await _productAppService.CreateAsync(request));

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] ProductRequestDTO request)
            => BaseResponse(await _productAppService.UpdateAsync(request, id));

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteAsync(Guid id)
            => BaseResponse(await _productAppService.DeleteAsync(id));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductResponseDTO))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
            => BaseResponse(await _productAppService.GetByIdAsync(id));

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedResponse<ProductResponseDTO>))]
        public async Task<IActionResult> GetPaginatedAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int itemsPerPage = 10,
            [FromQuery] string? search = "",
            [FromQuery] ProductSearchField? field = ProductSearchField.Name)
            => BaseResponse(await _productAppService.GetPaginatedAsync(pageNumber, itemsPerPage, search, field));
    }
}
