using ECommerce.API.Models.Requests.Order;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
  {
    public OrderItemRequestValidator()
    {
      RuleFor(x => x.ProductId).NotEmpty();
      RuleFor(x => x.Quantity).GreaterThan(0);
    }
  }
} 