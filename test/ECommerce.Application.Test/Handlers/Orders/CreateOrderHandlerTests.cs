using ECommerce.Application.Handlers.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Application.Test.Builders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Order;
using ECommerce.Domain.Models.Product;
using ECommerce.Domain.Models.User;
using FluentAssertions;
using NSubstitute;

namespace ECommerce.Application.Test.Handlers.Orders
{
  [TestFixture]
  public class CreateOrderHandlerTests
  {
    private IOrderRepository _orderRepo;
    private IProductRepository _productRepo;
    private ICustomerRepository _customerRepo;
    private IOrderService _orderService;
    private IAddressFactory _addressFactory;
    private CreateOrderHandler _handler;

    [SetUp]
    public void Setup()
    {
      _orderRepo = Substitute.For<IOrderRepository>();
      _productRepo = Substitute.For<IProductRepository>();
      _customerRepo = Substitute.For<ICustomerRepository>();
      _orderService = Substitute.For<IOrderService>();
      _addressFactory = Substitute.For<IAddressFactory>();

      _handler = new CreateOrderHandler(_orderRepo, _productRepo, _customerRepo, _orderService, _addressFactory);
    }

    [Test]
    public async Task Handle_ShouldCreateOrderSuccessfully()
    {
      // Arrange
      var request = new CreateOrderCommand
      {
        CustomerId = Guid.NewGuid(),
        Items =
            [
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 2 }
            ],
        ShippingAddress = new CustomerShippingAddressCommand(),
        BillingAddress = new OrderBillingAddressCommand()
      };

      var customer = new CustomerBuilder().WithId(request.CustomerId).Build();
      var product = new ProductBuilder().WithId(request.Items.FirstOrDefault().ProductId).Build();
      var result = new CreateOrderResult { OrderId = Guid.NewGuid() };

      _customerRepo.GetCustomerByIdAsync(request.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
      _productRepo.GetProductByIdAsync(request.Items[0].ProductId, Arg.Any<CancellationToken>()).Returns(product);
      _addressFactory.CreateShippingAddress(Arg.Any<CustomerShippingAddressCommand>(), request.CustomerId).Returns(new OrderShippingAddress());
      _addressFactory.CreateBillingAddress(Arg.Any<OrderBillingAddressCommand>(), request.CustomerId).Returns(new OrderBillingAddress());
      _orderService.CreateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>()).Returns(result);

      // Act
      var response = await _handler.Handle(request, CancellationToken.None);

      // Assert
      response.Should().Be(result);
      await _orderService.Received(1).CreateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_ShouldThrow_WhenCustomerNotFound()
    {
      // Arrange
      var command = new CreateOrderCommand
      {
        CustomerId = Guid.NewGuid(),
        Items = new List<OrderItemCommand>
              {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 1 }
              },
        ShippingAddress = new CustomerShippingAddressCommand(),
        BillingAddress = new OrderBillingAddressCommand()
      };

      _customerRepo.GetCustomerByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
          .Returns((Customer)null);

      // Act
      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>()
        .WithMessage($"Customer with ID {command.CustomerId} not found.");
    }

    [Test]
    public async Task Handle_ShouldThrow_WhenProductNotFound()
    {
      // Arrange
      var command = new CreateOrderCommand
      {
        CustomerId = Guid.NewGuid(),
        Items = new List<OrderItemCommand>
              {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 1 }
              },
        ShippingAddress = new CustomerShippingAddressCommand(),
        BillingAddress = new OrderBillingAddressCommand()
      };

      var customer = new CustomerBuilder().WithId(command.CustomerId).Build();

      _customerRepo.GetCustomerByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
          .Returns(customer);

      _productRepo.GetProductByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>())
          .Returns((Product)null);

      // Act
      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>()
        .WithMessage($"Product with ID {command.Items[0].ProductId} not found.");
    }

    [Test]
    public async Task Handle_ShouldThrow_WhenNoOrderItems()
    {
      // Arrange
      var command = new CreateOrderCommand
      {
        CustomerId = Guid.NewGuid(),
        Items = [],
        ShippingAddress = new CustomerShippingAddressCommand(),
        BillingAddress = new OrderBillingAddressCommand()
      };

      var customer = new CustomerBuilder().WithId(command.CustomerId).Build();
      _customerRepo.GetCustomerByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
          .Returns(customer);

      // Act
      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>()
        .WithMessage("Order must contain at least one product.");
    }

    [Test]
    public async Task Handle_ShouldThrow_WhenShippingAddressIsNull()
    {
      // Arrange
      var command = new CreateOrderCommand
      {
        CustomerId = Guid.NewGuid(),
        Items = [new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 1 }],
        ShippingAddress = null,
        BillingAddress = new OrderBillingAddressCommand()
      };

      var customer = new CustomerBuilder().WithId(command.CustomerId).Build();
      var product = new ProductBuilder().WithId(command.Items[0].ProductId).Build();

      _customerRepo.GetCustomerByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
          .Returns(customer);
      _productRepo.GetProductByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>())
          .Returns(product);

      _addressFactory.CreateShippingAddress(null, command.CustomerId).Returns((OrderShippingAddress)null);

      // Act
      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>()
        .WithMessage("Shipping address is required.");
    }

    [Test]
    public async Task Handle_ShouldThrow_WhenBillingAddressIsNull()
    {
      // Arrange
      var command = new CreateOrderCommand
      {
        CustomerId = Guid.NewGuid(),
        Items = [new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 1 }],
        ShippingAddress = new CustomerShippingAddressCommand(),
        BillingAddress = null
      };

      var customer = new CustomerBuilder().WithId(command.CustomerId).Build();
      var product = new ProductBuilder().WithId(command.Items[0].ProductId).Build();

      _customerRepo.GetCustomerByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
          .Returns(customer);
      _productRepo.GetProductByIdAsync(command.Items[0].ProductId, Arg.Any<CancellationToken>())
          .Returns(product);

      _addressFactory.CreateShippingAddress(Arg.Any<CustomerShippingAddressCommand>(), command.CustomerId).Returns(new OrderShippingAddress());
      _addressFactory.CreateBillingAddress(null, command.CustomerId).Returns((OrderBillingAddress)null);

      // Act
      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>()
        .WithMessage("Billing address is required.");
    }
  }
}
