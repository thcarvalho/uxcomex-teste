using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UXComex.API.Controllers.Abstract;
using UXComex.Application.Interfaces.Services;
using UXComex.Domain.DTOs.Client;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;

namespace UXComex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(IClientAppService clientAppService) : BaseApiController
    {
        private readonly IClientAppService _clientAppService = clientAppService;

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ClientResponseDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> PostAsync([FromBody] ClientRequestDTO request)
            => BaseResponse(await _clientAppService.CreateAsync(request));

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ClientResponseDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] ClientRequestDTO request)
            => BaseResponse(await _clientAppService.UpdateAsync(request, id));

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteAsync(Guid id)
            => BaseResponse(await _clientAppService.DeleteAsync(id));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ClientResponseDto))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
            => BaseResponse(await _clientAppService.GetByIdAsync(id));

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedResponse<ClientResponseDto>))]
        public async Task<IActionResult> GetPaginatedAsync(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int itemsPerPage = 10,
            [FromQuery] string? search = "",
            [FromQuery] ClientSearchField? field = ClientSearchField.Name)
            => BaseResponse(await _clientAppService.GetPaginatedAsync(pageNumber, itemsPerPage, search, field));
    }
}
