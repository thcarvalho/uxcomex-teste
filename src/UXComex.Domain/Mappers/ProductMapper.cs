using UXComex.Domain.DTOs.Product;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Mappers;

public static class ProductMapper
{
    public static Product ToEntity(this ProductRequestDTO productDto)
        => new Product(
            productDto.Name,
            productDto.Description,
            productDto.Price,
            productDto.QuantityInStock);

    public static ProductResponseDTO ToDTO(this Product product)
        => new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock
        };
}