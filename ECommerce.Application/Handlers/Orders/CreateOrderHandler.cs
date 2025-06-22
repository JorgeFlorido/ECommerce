using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
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

      var orderItems = new List<OrderItem>();

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
          UnitPrice = product.Price
        });
      }

      var order = new Order
      {
        CustomerId = request.CustomerId,
        OrderDate = DateTime.UtcNow,
        Status = OrderStatus.Pending,
        Items = orderItems,
        ShippingAddress = _addressFactory.CreateShippingAddress(request.ShippingAddress, request.CustomerId),
        BillingAddress = _addressFactory.CreateBillingAddress(request.BillingAddress, request.CustomerId),
        DiscountCode = null 
      };

      var orderResult = await _orderService.CreateOrderAsync(order, cancellationToken);
      
      return orderResult;
    }
  }
} 