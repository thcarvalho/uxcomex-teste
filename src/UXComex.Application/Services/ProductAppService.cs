using UXComex.Application.Exceptions;
using UXComex.Application.Interfaces.Services;
using UXComex.Domain.DTOs.Product;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;
using UXComex.Domain.Mappers;
using UXComex.Domain.Repositories;

namespace UXComex.Application.Services;

public class ProductAppService(IProductRepository productRepository) : IProductAppService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<PaginatedResponse<ProductResponseDTO>> GetPaginatedAsync(
        int pageNumber = 1, 
        int itemsPerPage = 10, 
        string? search = "",
        ProductSearchField? field = null)
    {
        var response = await _productRepository.GetPaginatedAsync(
            pageNumber, itemsPerPage, search, Enum.GetName(typeof(ProductSearchField), field));

        var count = await _productRepository.CountAsync(search, Enum.GetName(typeof(ProductSearchField), field));

        return new PaginatedResponse<ProductResponseDTO>
        {
            Items = response.Select(x => x.ToDTO()),
            ItemsPerPage = itemsPerPage,
            Page = pageNumber,
            TotalPages = (int)Math.Ceiling((double)count / itemsPerPage)
        };
    }

    public async Task<ProductResponseDTO> GetByIdAsync(Guid id)
    {
        var response = await _productRepository.GetByIdAsync(id);
        if (response is null)
            throw new NotFoundException($"Product with ID {id} not found.");
        return response.ToDTO();
    }

    public async Task<ProductResponseDTO> CreateAsync(ProductRequestDTO product)
    {
        var entity = product.ToEntity();
        if (entity.IsInvalid())
            throw new ValidationException(entity.ErrorsToString());

        var response = await _productRepository.CreateAsync(entity);
        return response.ToDTO();
    }

    public async Task<ProductResponseDTO> UpdateAsync(ProductRequestDTO product, Guid id)
    {
        var entity = product.ToEntity();
        if (entity.IsInvalid())
            throw new ValidationException(entity.ErrorsToString());

        var current = await _productRepository.GetByIdAsync(id);
        if (current is null)
            throw new NotFoundException($"Product with ID {id} not found.");

        var response = await _productRepository.UpdateAsync(entity, id);
        return response.ToDTO();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var current = await _productRepository.GetByIdAsync(id);
        if (current is null)
            throw new NotFoundException($"Product with ID {id} not found.");

        return await _productRepository.DeleteAsync(id);
    }

    public async Task<ProductResponseDTO> ChangeProductStockAsync(Guid id, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException($"Product with ID {id} not found.");

        product.ChangeStockQuantity(quantity);
        
        var response = await _productRepository.UpdateAsync(product, id);
        return response.ToDTO();
    }
}