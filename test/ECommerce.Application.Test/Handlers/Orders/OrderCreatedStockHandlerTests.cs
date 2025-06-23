using ECommerce.Application.Handlers.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Events;
using ECommerce.Domain.Models.Order;
using FluentAssertions;
using NSubstitute;

namespace ECommerce.Application.Test.Handlers.Orders
{
  [TestFixture]
  public class OrderCreatedStockHandlerTests
  {
    private IOrderRepository _orderRepository;
    private IInventoryService _inventoryService;
    private OrderCreatedStockHandler _handler;

    [SetUp]
    public void Setup()
    {
      _orderRepository = Substitute.For<IOrderRepository>();
      _inventoryService = Substitute.For<IInventoryService>();
      _handler = new OrderCreatedStockHandler(_orderRepository, _inventoryService);
    }

    [Test]
    public async Task GivenValidOrder_WhenHandlingOrderCreatedEvent_ThenShouldUpdateStock()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var orderItems = new List<OrderItem>
      {
        new OrderItem { ProductId = Guid.NewGuid(), Quantity = 2 },
        new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1 }
      };
      
      var order = new Order { Id = orderId, Items = orderItems };
      var @event = new OrderCreatedEvent(orderId, Guid.NewGuid());

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);

      // Act
      await _handler.Handle(@event, CancellationToken.None);

      // Assert
      foreach (var item in orderItems)
      {
        await _inventoryService.Received(1).UpdateProductStockAsync(
          item.ProductId,
          -item.Quantity,
          Arg.Any<CancellationToken>()
        );
      }
    }

    [Test]
    public async Task GivenNonExistentOrder_WhenHandlingOrderCreatedEvent_ThenShouldNotUpdateStock()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var @event = new OrderCreatedEvent(orderId, Guid.NewGuid());

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns((Order)null);

      // Act
      await _handler.Handle(@event, CancellationToken.None);

      // Assert
      await _inventoryService.DidNotReceive().UpdateProductStockAsync(
        Arg.Any<Guid>(),
        Arg.Any<int>(),
        Arg.Any<CancellationToken>()
      );
    }

    [Test]
    public async Task GivenOrderWithNoItems_WhenHandlingOrderCreatedEvent_ThenShouldNotUpdateStock()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var order = new Order { Id = orderId, Items = new List<OrderItem>() };
      var @event = new OrderCreatedEvent(orderId, Guid.NewGuid());

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);

      // Act
      await _handler.Handle(@event, CancellationToken.None);

      // Assert
      await _inventoryService.DidNotReceive().UpdateProductStockAsync(
        Arg.Any<Guid>(),
        Arg.Any<int>(),
        Arg.Any<CancellationToken>()
      );
    }
  }
} 