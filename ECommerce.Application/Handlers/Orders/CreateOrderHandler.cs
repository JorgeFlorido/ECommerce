using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Orders;
using MediatR;
using AutoMapper;

namespace ECommerce.Application.Handlers.Orders
{
  internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    
    public CreateOrderHandler(
      IOrderRepository orderRepository,
      IProductRepository productRepository,
      ICustomerRepository customerRepository,
      IOrderService orderService,
      IMapper mapper)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        var orderItem = _mapper.Map<OrderItem>(item);
        orderItem.UnitPrice = product.Price;
        orderItem.TotalPrice = product.Price * item.Quantity;
        orderItems.Add(orderItem);
        totalAmount += orderItem.TotalPrice;
      }

      // Use AutoMapper for address mapping
      var shippingAddress = MapShippingAddress(request.ShippingAddress, request.CustomerId);
      var billingAddress = MapBillingAddress(request.BillingAddress, request.CustomerId);

      var order = new Order
      {
        CustomerId = request.CustomerId,
        OrderDate = DateTime.UtcNow,
        Status = OrderStatus.Pending,
        TotalAmount = totalAmount,
        Items = orderItems,
        ShippingAddress = shippingAddress,
        BillingAddress = billingAddress
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
        CustomerShippingAddressCommand customerCommand => _mapper.Map<CustomerAddress>(customerCommand.Address),
        DeliveryPointShippingAddressCommand deliveryCommand => _mapper.Map<DeliveryPointAddress>(deliveryCommand.Address),
        LockerShippingAddressCommand lockerCommand => _mapper.Map<LockerAddress>(lockerCommand.Address),
        _ => throw new ArgumentException($"Unsupported address type: {command.GetType().Name}")
      };

      return new OrderShippingAddress
      {
        Id = Guid.NewGuid(),
        Type = command.Type,
        Address = address
      };
    }

    private OrderBillingAddress? MapBillingAddress(OrderBillingAddressCommand? command, Guid customerId)
    {
      if (command == null) return null;

      return new OrderBillingAddress
      {
        Id = Guid.NewGuid(),
        CustomerAddress = _mapper.Map<CustomerAddress>(command.CustomerAddress)
      };
    }
  }
} 