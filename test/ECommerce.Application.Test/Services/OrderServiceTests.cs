using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Application.Services;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.Order;
using FluentAssertions;
using NSubstitute;

namespace ECommerce.Application.Tests.Services
{
  [TestFixture]
  public class OrderServiceTests
  {
    private IOrderRepository _orderRepository;
    private ICheckoutProcessor _checkoutProcessor;
    private IOrderPaymentProcessor _paymentProcessor;
    private OrderService _service;

    [SetUp]
    public void SetUp()
    {
      _orderRepository = Substitute.For<IOrderRepository>();
      _checkoutProcessor = Substitute.For<ICheckoutProcessor>();
      _paymentProcessor = Substitute.For<IOrderPaymentProcessor>();
      _service = new OrderService(_orderRepository, _checkoutProcessor, _paymentProcessor);
    }

    [Test]
    public async Task CalculateOrderCostAsync_ShouldReturnResult_FromCheckoutProcessor()
    {
      // Arrange
      var query = new OrderCostCalculationQuery();
      var expected = new OrderCostCalculationResult { GrossAmount = 100m };
      _checkoutProcessor.CalculateOrderCostAsync(query, Arg.Any<CancellationToken>())
          .Returns(expected);

      // Act
      var result = await _service.CalculateOrderCostAsync(query);

      // Assert
      result.Should().Be(expected);
    }

    [Test]
    public async Task CreateOrderAsync_ShouldReturnFailure_WhenOutOfStock()
    {
      // Arrange
      var order = new Order { Items = new List<OrderItem>() };
      var costResult = new OrderCostCalculationResult();
      var outOfStock = new List<Guid> { Guid.NewGuid() };

      _checkoutProcessor.CalculateOrderCostAsync(Arg.Any<OrderCostCalculationQuery>(), Arg.Any<CancellationToken>())
          .Returns(costResult);
      _checkoutProcessor.GetOutOfStockItemsAsync(order.Items, Arg.Any<CancellationToken>())
          .Returns(outOfStock);

      // Act
      var result = await _service.CreateOrderAsync(order);

      // Assert
      result.Success.Should().BeFalse();
      result.OutOfStockItemIds.Should().BeEquivalentTo(outOfStock);
    }

    [Test]
    public async Task CreateOrderAsync_ShouldReturnSuccess_WhenAllInStock()
    {
      // Arrange
      var order = new Order { Id = Guid.NewGuid(), Items = new List<OrderItem>() };
      var costResult = new OrderCostCalculationResult();
      _checkoutProcessor.CalculateOrderCostAsync(Arg.Any<OrderCostCalculationQuery>(), Arg.Any<CancellationToken>())
          .Returns(costResult);
      _checkoutProcessor.GetOutOfStockItemsAsync(order.Items, Arg.Any<CancellationToken>())
          .Returns(new List<Guid>());

      // Act
      var result = await _service.CreateOrderAsync(order);

      // Assert
      result.Success.Should().BeTrue();
      result.OrderId.Should().Be(order.Id);
      await _orderRepository.Received(1).AddOrderAsync(order, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ProcessOrderAsync_ShouldReturnFalse_WhenOrderNotFound()
    {
      // Arrange
      _orderRepository.GetOrderByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
          .Returns((Order)null);

      // Act
      var result = await _service.ProcessOrderAsync(Guid.NewGuid());

      // Assert
      result.Should().BeFalse();
    }

    [Test]
    public async Task ProcessOrderAsync_ShouldReturnFalse_WhenPaymentFails()
    {
      // Arrange
      var order = new Order();
      _orderRepository.GetOrderByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
          .Returns(order);
      _paymentProcessor.ProcessPaymentAsync(order, Arg.Any<CancellationToken>())
          .Returns(false);

      // Act
      var result = await _service.ProcessOrderAsync(Guid.NewGuid());

      // Assert
      result.Should().BeFalse();
    }

    [Test]
    public async Task ProcessOrderAsync_ShouldUpdateOrderStatus_WhenPaymentSucceeds()
    {
      // Arrange
      var order = new Order();
      _orderRepository.GetOrderByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
          .Returns(order);
      _paymentProcessor.ProcessPaymentAsync(order, Arg.Any<CancellationToken>())
          .Returns(true);

      // Act
      var result = await _service.ProcessOrderAsync(Guid.NewGuid());

      // Assert
      result.Should().BeTrue();
      order.Status.Should().Be(OrderStatus.Processing);
      await _orderRepository.Received(1).UpdateOrderAsync(order, Arg.Any<CancellationToken>());
    }
  }
}