using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class OrderBillingAddressRequestValidator : AbstractValidator<OrderBillingAddressRequest>
  {
    public OrderBillingAddressRequestValidator()
    {
      RuleFor(x => x.CustomerAddress).SetValidator(new CustomerAddressRequestValidator());
    }
  }
} 