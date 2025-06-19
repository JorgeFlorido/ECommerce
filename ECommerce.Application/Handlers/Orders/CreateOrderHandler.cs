using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Orders;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderService _orderService;
    
    public CreateOrderHandler(
      IOrderRepository orderRepository,
      IProductRepository productRepository,
      ICustomerRepository customerRepository,
      IOrderService orderService)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }
    
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
      // Validate customer exists
      var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
      if (customer == null)
      {
        throw new ArgumentException($"Customer with ID {request.CustomerId} not found.");
      }

      // Validate products and calculate total
      var orderItems = new List<OrderItem>();
      decimal totalAmount = 0;

      foreach (var item in request.Items)
      {
        var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
        if (product == null)
        {
          throw new ArgumentException($"Product with ID {item.ProductId} not found.");
        }

        orderItems.Add(new OrderItem
        {
          ProductId = item.ProductId,
          Quantity = item.Quantity,
          UnitPrice = product.Price,
          TotalPrice = product.Price * item.Quantity
        });

        totalAmount += product.Price * item.Quantity;
      }

      // Create order with proper address mapping
      var order = new Order
      {
        CustomerId = request.CustomerId,
        OrderDate = DateTime.UtcNow,
        Status = OrderStatus.Pending,
        TotalAmount = totalAmount,
        Items = orderItems,
        ShippingAddress = MapShippingAddress(request.ShippingAddress, request.CustomerId),
        BillingAddress = MapBillingAddress(request.BillingAddress, request.CustomerId)
      };

      // Use order service for complex business logic
      var orderId = await _orderService.CreateOrderAsync(order, cancellationToken);
      
      return orderId;
    }

    private OrderShippingAddress? MapShippingAddress(OrderShippingAddressCommand? command, Guid customerId)
    {
      if (command == null) return null;

      Address address = command switch
      {
        CustomerShippingAddressCommand customerCommand => MapToCustomerAddress(customerCommand.Address, customerId),
        DeliveryPointShippingAddressCommand deliveryCommand => MapToDeliveryPointAddress(deliveryCommand.Address),
        LockerShippingAddressCommand lockerCommand => MapToLockerAddress(lockerCommand.Address),
        _ => throw new ArgumentException($"Unsupported address type: {command.GetType().Name}")
      };

      return new OrderShippingAddress
      {
        Id = Guid.NewGuid(),
        Type = command.Type,
        Address = address
      };
    }

    private CustomerAddress MapToCustomerAddress(CustomerAddressCommand command, Guid customerId)
    {
      return new CustomerAddress
      {
        Id = Guid.NewGuid(),
        CustomerId = customerId,
        Street = command.Street,
        City = command.City,
        State = command.State,
        PostalCode = command.PostalCode,
        Country = command.Country,
        IsPrimary = command.IsPrimary
      };
    }

    private DeliveryPointAddress MapToDeliveryPointAddress(DeliveryPointAddressCommand command)
    {
      return new DeliveryPointAddress
      {
        Id = Guid.NewGuid(),
        Street = command.Street,
        City = command.City,
        State = command.State,
        PostalCode = command.PostalCode,
        Country = command.Country,
        ShopName = command.ShopName,
        ContactNumber = command.ContactNumber
      };
    }

    private LockerAddress MapToLockerAddress(LockerAddressCommand command)
    {
      return new LockerAddress
      {
        Id = Guid.NewGuid(),
        Street = command.Street,
        City = command.City,
        State = command.State,
        PostalCode = command.PostalCode,
        Country = command.Country,
        LockerId = command.LockerId,
        Provider = command.Provider
      };
    }

    private OrderBillingAddress? MapBillingAddress(OrderBillingAddressCommand? command, Guid customerId)
    {
      if (command == null) return null;

      return new OrderBillingAddress
      {
        Id = Guid.NewGuid(),
        CustomerAddress = new CustomerAddress
        {
          Id = Guid.NewGuid(),
          CustomerId = customerId,
          Street = command.CustomerAddress.Street,
          City = command.CustomerAddress.City,
          State = command.CustomerAddress.State,
          PostalCode = command.CustomerAddress.PostalCode,
          Country = command.CustomerAddress.Country,
          IsPrimary = command.CustomerAddress.IsPrimary
        }
      };
    }
  }
} 