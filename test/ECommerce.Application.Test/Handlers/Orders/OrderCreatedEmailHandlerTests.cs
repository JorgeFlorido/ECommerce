using ECommerce.Application.Handlers.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Events;
using FluentAssertions;
using NSubstitute;

namespace ECommerce.Application.Test.Handlers.Orders
{
  [TestFixture]
  public class OrderCreatedEmailHandlerTests
  {
    private INotificationService _notificationService;
    private OrderCreatedEmailHandler _handler;

    [SetUp]
    public void Setup()
    {
      _notificationService = Substitute.For<INotificationService>();
      _handler = new OrderCreatedEmailHandler(_notificationService);
    }

    [Test]
    public async Task GivenOrderCreatedEvent_WhenHandlingEvent_ThenShouldSendNotification()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var customerId = Guid.NewGuid();
      var @event = new OrderCreatedEvent(orderId, customerId);
      var expectedMessage = $"Your order {orderId} has been created.";

      // Act
      await _handler.Handle(@event, CancellationToken.None);

      // Assert
      await _notificationService.Received(1).SendNotificationAsync(
        customerId,
        expectedMessage
      );
    }

    [Test]
    public async Task GivenOrderCreatedEvent_WhenNotificationFails_ThenShouldNotThrowException()
    {
      // Arrange
      var orderId = Guid.NewGuid();
      var customerId = Guid.NewGuid();
      var @event = new OrderCreatedEvent(orderId, customerId);

      _notificationService
        .SendNotificationAsync(Arg.Any<Guid>(), Arg.Any<string>())
        .Returns(Task.FromException(new Exception("Notification failed")));

      // Act
      Func<Task> action = async () => await _handler.Handle(@event, CancellationToken.None);

      // Assert
      await action.Should().NotThrowAsync();
    }
  }
} 