using ECommerce.Application.Interfaces;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Events;

namespace ECommerce.Application.Handlers.Orders
{
    public class OrderCreatedStockHandler : IDomainEventHandler<OrderCreatedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryService _inventoryService;

        public OrderCreatedStockHandler(IOrderRepository orderRepository, IInventoryService inventoryService)
        {
            _orderRepository = orderRepository;
            _inventoryService = inventoryService;
        }

        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(domainEvent.OrderId, cancellationToken);
            if (order == null) return;

            foreach (var item in order.Items)
            {
                await _inventoryService.UpdateProductStockAsync(item.ProductId, -item.Quantity, cancellationToken);
            }
        }
    }
} 