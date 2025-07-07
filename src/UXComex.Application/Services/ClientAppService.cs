using UXComex.Application.Exceptions;
using UXComex.Application.Interfaces.Services;
using UXComex.Domain.DTOs.Client;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;
using UXComex.Domain.Mappers;
using UXComex.Domain.Repositories;

namespace UXComex.Application.Services;

public class ClientAppService(IClientRepository clientRepository) : IClientAppService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<PaginatedResponse<ClientResponseDto>> GetPaginatedAsync(
        int pageNumber = 1, 
        int itemsPerPage = 10, 
        string? search = "", 
        ClientSearchField? field = null)
    {
        var response = await _clientRepository.GetPaginatedAsync(
            pageNumber, itemsPerPage, search, Enum.GetName(typeof(ClientSearchField), field));

        var count = await _clientRepository.CountAsync(search, Enum.GetName(typeof(ClientSearchField), field));

        return new PaginatedResponse<ClientResponseDto>
        {
            Items = response.Select(x => x.ToDTO()),
            ItemsPerPage = itemsPerPage,
            Page = pageNumber,
            TotalPages = (int)Math.Ceiling((double)count / itemsPerPage)
        };
    }

    public async Task<ClientResponseDto> GetByIdAsync(Guid id)
    {
        var response = await _clientRepository.GetByIdAsync(id);
        if (response is null)
            throw new NotFoundException($"Client with ID {id} not found.");
        return response.ToDTO();
    }

    public async Task<ClientResponseDto> CreateAsync(ClientRequestDTO client)
    {
        var entity = client.ToEntity();
        if (entity.IsInvalid())
            throw new ValidationException(entity.ErrorsToString());

        if (await _clientRepository.ExistsEmailAsync(entity.Email))
            throw new ValidationException($"Client with email {entity.Email} already exists.");

        var response = await _clientRepository.CreateAsync(entity);
        return response.ToDTO();
    }

    public async Task<ClientResponseDto> UpdateAsync(ClientRequestDTO client, Guid id)
    {
        var entity = client.ToEntity();
        if (entity.IsInvalid())
            throw new ValidationException(entity.ErrorsToString());

        var current = await _clientRepository.GetByIdAsync(id);
        if (current is null)
            throw new NotFoundException($"Client with ID {id} not found.");

        if (await _clientRepository.ExistsEmailAsync(entity.Email, current.Email))
            throw new ValidationException($"Client with email {entity.Email} already exists.");

        var response = await _clientRepository.UpdateAsync(entity, id);
        return response.ToDTO();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var current = await _clientRepository.GetByIdAsync(id);
        if (current is null)
            throw new NotFoundException($"Client with ID {id} not found.");

        return await _clientRepository.DeleteAsync(id);
    }
}