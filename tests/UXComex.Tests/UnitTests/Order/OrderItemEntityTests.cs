namespace UXComex.Tests.UnitTests.Order;

public class OrderItemEntityTests
{
    [Fact]
    public void OrderItemEntity_Validate_ShouldAddErrorsToEntity_WhenInvalid()
    {
        // Arrange
        var orderItem = new Domain.Entities.OrderItem(Guid.Empty, Guid.Empty, 0, 0);

        // Act & Assert
        Assert.True(orderItem.IsInvalid());
        Assert.NotEmpty(orderItem.ErrorsToString());
    }

    [Fact]
    public void OrderItemEntity_Validate_ShouldNotAddErrorsToEntity_WhenValid()
    {
        // Arrange
        var orderItem = new Domain.Entities.OrderItem(Guid.NewGuid(), Guid.NewGuid(), 2, 3m);

        // Act & Assert
        Assert.False(orderItem.IsInvalid());
        Assert.Empty(orderItem.ErrorsToString());
    }
}