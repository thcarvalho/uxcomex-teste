using ValidationException = FluentValidation.ValidationException;

namespace UXComex.Tests.UnitTests.Order;

public class OrderEntityTests
{
    [Fact]
    public void OrderEntity_Validate_ShouldAddErrorsToEntity_WhenInvalid()
    {
        // Arrange
        var order = new Domain.Entities.Order(Guid.Empty);

        // Act & Assert
        Assert.True(order.IsInvalid());
        Assert.NotEmpty(order.ErrorsToString());
    }

    [Fact]
    public void OrderEntity_Validate_ShouldNotAddErrorsToEntity_WhenValid()
    {
        // Arrange
        var order = new Domain.Entities.Order(Guid.NewGuid());

        // Act & Assert
        Assert.False(order.IsInvalid());
        Assert.Empty(order.ErrorsToString());
    }

    [Fact]
    public void OrderEntity_CalculateTotalAmount_ShouldCalculateCorrectTotal_WhenOrderItemsAdded()
    {
        // Arrange
        var order = new Domain.Entities.Order(Guid.NewGuid());
        order.AddOrderItem(Guid.NewGuid(), 2, 50.00m);
        order.AddOrderItem(Guid.NewGuid(), 1, 100.00m);

        // Act
        order.CalculateTotalAmount();
        
        // Assert
        Assert.Equal(200.00m, order.TotalAmount);
    }

    [Fact]
    public void OrderEntity_ChangeStatus_ShouldUpdateStatus_WhenCalled()
    {
        // Arrange
        var order = new Domain.Entities.Order(Guid.NewGuid());
        var newStatus = Domain.Enums.OrderStatus.Delivered;

        // Act
        order.ChangeStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, order.Status);
    }

    [Fact]
    public void OrderEntity_AddOrderItem_ShouldAddItemToOrder_WhenCalled()
    {
        // Arrange
        var order = new Domain.Entities.Order(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var quantity = 3;
        var price = 20.00m;

        // Act
        var orderItem = order.AddOrderItem(productId, quantity, price);

        // Assert
        Assert.Contains(orderItem, order.OrderItems);
        Assert.Equal(quantity * price, order.TotalAmount);
    }
}