using ECommerce.Application.Handlers.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Order;
using FluentAssertions;
using NSubstitute;

namespace ECommerce.Application.Test.Handlers.Orders
{
  [TestFixture]
  public class CancelOrderHandlerTests
  {
    private IOrderRepository _orderRepository;
    private IOrderService _orderService;
    private CancelOrderHandler _handler;

    [SetUp]
    public void Setup()
    {
      _orderRepository = Substitute.For<IOrderRepository>();
      _orderService = Substitute.For<IOrderService>();
      _handler = new CancelOrderHandler(_orderRepository, _orderService);
    }

    [Test]
    public async Task GivenValidOrderId_WhenCancellingOrder_ThenShouldReturnSuccess()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var request = new CancelOrderCommand 
      { 
        OrderId = orderId,
        Reason = "Customer request",
        RefundPayment = true
      };
      
      var order = new Order { Id = orderId };
      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);
      _orderService.CancelOrderAsync(orderId, "Customer request", true, Arg.Any<CancellationToken>()).Returns(true);

      // Act
      var result = await _handler.Handle(request, CancellationToken.None);

      // Assert
      result.Should().BeTrue();
      await _orderService.Received(1).CancelOrderAsync(orderId, "Customer request", true, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GivenNonExistentOrderId_WhenCancellingOrder_ThenShouldThrowArgumentException()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var request = new CancelOrderCommand 
      { 
        OrderId = orderId,
        Reason = "Customer request",
        RefundPayment = true
      };

      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns((Order)null);

      // Act
      Func<Task> action = async () => await _handler.Handle(request, CancellationToken.None);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>()
          .WithMessage($"Order with ID {orderId} not found.");
    }

    [Test]
    public async Task GivenCancellationFailure_WhenCancellingOrder_ThenShouldReturnFalse()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var request = new CancelOrderCommand 
      { 
        OrderId = orderId,
        Reason = "Customer request",
        RefundPayment = true
      };
      
      var order = new Order { Id = orderId };
      _orderRepository.GetOrderByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);
      _orderService.CancelOrderAsync(orderId, "Customer request", true, Arg.Any<CancellationToken>()).Returns(false);

      // Act
      var result = await _handler.Handle(request, CancellationToken.None);

      // Assert
      result.Should().BeFalse();
    }
  }
} 