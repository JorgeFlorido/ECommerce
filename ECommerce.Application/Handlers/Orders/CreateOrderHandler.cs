using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Events;
using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderService _orderService;
    private readonly IAddressFactory _addressFactory;

    public CreateOrderHandler(
      IOrderRepository orderRepository,
      IProductRepository productRepository,
      ICustomerRepository customerRepository,
      IOrderService orderService, 
      IAddressFactory addressFactory)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
      _addressFactory = addressFactory ?? throw new ArgumentNullException(nameof(addressFactory));
    }
    
    public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
      if (customer == null)
      {
        throw new ArgumentException($"Customer with ID {request.CustomerId} not found.");
      }

      if (request.Items == null || !request.Items.Any())
      {
        throw new ArgumentException("Order must contain at least one product.");
      }

      if (request.ShippingAddress == null)
      {
        throw new ArgumentException("Shipping address is required.");
      }

      if (request.BillingAddress == null)
      {
        throw new ArgumentException("Billing address is required.");
      }

      var orderItems = new List<OrderItem>();

      foreach (var item in request.Items)
      {
        if (item.Quantity <= 0)
        {
          throw new ArgumentException("Order item quantity must be greater than zero.");
        }

        var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
        if (product == null)
        {
          throw new ArgumentException($"Product with ID {item.ProductId} not found.");
        }

        orderItems.Add(new OrderItem
        {
          ProductId = item.ProductId,
          Quantity = item.Quantity,
          UnitPrice = product.Price
        });
      }

      var shippingAddress = _addressFactory.CreateShippingAddress(request.ShippingAddress, request.CustomerId);
      if (shippingAddress == null)
      {
        throw new ArgumentException("Invalid shipping address data.");
      }

      var billingAddress = _addressFactory.CreateBillingAddress(request.BillingAddress, request.CustomerId);
      if (billingAddress == null)
      {
        throw new ArgumentException("Invalid billing address data.");
      }

      var order = new Order
      {
        CustomerId = request.CustomerId,
        OrderDate = DateTime.UtcNow,
        Status = OrderStatus.Pending,
        Items = orderItems,
        ShippingAddress = shippingAddress,
        BillingAddress = billingAddress,
        DiscountCode = null
      };

      order.AddDomainEvent(new OrderCreatedEvent(order.Id, order.CustomerId));

      var orderResult = await _orderService.CreateOrderAsync(order, cancellationToken);

      return orderResult;
    }
  }
} 