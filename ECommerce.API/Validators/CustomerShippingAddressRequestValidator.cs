using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class CustomerShippingAddressRequestValidator : AbstractValidator<CustomerShippingAddressRequest>
  {
    public CustomerShippingAddressRequestValidator()
    {
      RuleFor(x => x.Address).SetValidator(new CustomerAddressRequestValidator());
    }
  }
} 