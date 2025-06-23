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
    public async Task GivenValidOrderCostQuery_WhenCalculatingOrderCost_ThenShouldReturnProcessorResult()
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
    public async Task GivenOutOfStockItems_WhenCreatingOrder_ThenShouldReturnFailure()
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
    public async Task GivenAllItemsInStock_WhenCreatingOrder_ThenShouldReturnSuccess()
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
    public async Task GivenNonExistentOrderId_WhenProcessingOrder_ThenShouldReturnFalse()
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
    public async Task GivenFailedPayment_WhenProcessingOrder_ThenShouldReturnFalse()
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
    public async Task GivenSuccessfulPayment_WhenProcessingOrder_ThenShouldUpdateStatusAndReturnTrue()
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

    [Test]
    public async Task GivenConcurrentRequests_WhenProcessingOrder_ThenShouldHandleConcurrencyCorrectly()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var order = new Order { Id = orderId };
      var concurrentOrder = new Order { Id = orderId };

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>())
          .Returns(order, concurrentOrder);
      _paymentProcessor.ProcessPaymentAsync(order, Arg.Any<CancellationToken>())
          .Returns(true);

      // Act
      var task1 = _service.ProcessOrderAsync(orderId);
      var task2 = _service.ProcessOrderAsync(orderId);

      // Assert
      var results = await Task.WhenAll(task1, task2);
      results.Should().Contain(true); // At least one should succeed
      await _orderRepository.Received(2).GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>());
      await _orderRepository.Received(1).UpdateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GivenPaymentTimeout_WhenProcessingOrder_ThenShouldReturnFalse()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var order = new Order { Id = orderId };
      var cts = new CancellationTokenSource();
      cts.Cancel(); // Simulate timeout

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>())
          .Returns(order);
      _paymentProcessor.ProcessPaymentAsync(order, cts.Token)
          .Returns(Task.FromCanceled<bool>(cts.Token));

      // Act
      var result = await _service.ProcessOrderAsync(orderId, cts.Token);

      // Assert
      result.Should().BeFalse();
      await _orderRepository.DidNotReceive().UpdateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GivenInvalidStatusTransition_WhenProcessingOrder_ThenShouldReturnFalse()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var order = new Order { Id = orderId, Status = OrderStatus.Canceled };

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>())
          .Returns(order);

      // Act
      var result = await _service.ProcessOrderAsync(orderId);

      // Assert
      result.Should().BeFalse();
      await _paymentProcessor.DidNotReceive().ProcessPaymentAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
      await _orderRepository.DidNotReceive().UpdateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GivenValidStatusTransition_WhenProcessingOrder_ThenShouldUpdateStatus()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var order = new Order { Id = orderId, Status = OrderStatus.Pending };

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>())
          .Returns(order);
      _paymentProcessor.ProcessPaymentAsync(order, Arg.Any<CancellationToken>())
          .Returns(true);

      // Act
      var result = await _service.ProcessOrderAsync(orderId);

      // Assert
      result.Should().BeTrue();
      order.Status.Should().Be(OrderStatus.Processing);
      await _orderRepository.Received(1).UpdateOrderAsync(order, Arg.Any<CancellationToken>());
    }
  }
}