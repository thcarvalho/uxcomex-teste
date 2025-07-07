using UXComex.Application.Exceptions;

namespace UXComex.Tests.UnitTests.Product;

public class ProductEntityTests
{
    [Fact]
    public void ProductEntity_Validate_ShouldAddErrorsToEntity_WhenInvalid()
    {
        // Arrange
        var product = new Domain.Entities.Product("", "", 0, 0);

        // Act & Assert
        Assert.True(product.IsInvalid());
        Assert.NotEmpty(product.ErrorsToString());
    }

    [Fact]
    public void ProductEntity_Validate_ShouldNotAddErrorsToEntity_WhenValid()
    {
        // Arrange
        var product = new Domain.Entities.Product("Produto", "Descrição do Produto", 30m, 10);

        // Act & Assert
        Assert.False(product.IsInvalid());
        Assert.Empty(product.ErrorsToString());
    }

    [Fact]
    public void ProductEntity_ChangeStockQuantity_ShouldThrowDomainException_WhenQuantityIsHigherThanStock()
    {
        // Arrange
        var product = new Domain.Entities.Product("Produto", "Descrição do Produto", 30m, 10);

        // Act & Assert
        Assert.Throws<DomainException>(() => product.ChangeStockQuantity(11));
    }

    [Fact]
    public void ProductEntity_ChangeStockQuantity_ShouldThrowDomainException_WhenNoStock()
    {
        // Arrange
        var product = new Domain.Entities.Product("Produto", "Descrição do Produto", 30m, 10);
        product.ChangeStockQuantity(10);

        // Act & Assert
        Assert.Throws<DomainException>(() => product.ChangeStockQuantity(1));
    }
}