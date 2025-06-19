using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class LockerShippingAddressRequestValidator : AbstractValidator<LockerShippingAddressRequest>
  {
    public LockerShippingAddressRequestValidator()
    {
      RuleFor(x => x.Address).SetValidator(new LockerAddressRequestValidator());
    }
  }
} 