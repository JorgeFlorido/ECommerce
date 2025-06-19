using ECommerce.API.Models.Requests.Order;
using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
  {
    public CreateOrderRequestValidator()
    {
      RuleFor(x => x.CustomerId).NotEmpty();
      RuleFor(x => x.Items).NotEmpty();
      RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
      RuleFor(x => x.ShippingAddress).NotNull().SetInheritanceValidator(v =>
      {
        v.Add(new CustomerShippingAddressRequestValidator());
        v.Add(new DeliveryPointShippingAddressRequestValidator());
        v.Add(new LockerShippingAddressRequestValidator());
      });
      RuleFor(x => x.BillingAddress).NotNull()
        .DependentRules(() => 
        {
          RuleFor(x => x.BillingAddress!).SetValidator(new OrderBillingAddressRequestValidator());
        });
    }
  }
} 