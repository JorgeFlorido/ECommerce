using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class DeliveryPointShippingAddressRequestValidator : AbstractValidator<DeliveryPointShippingAddressRequest>
  {
    public DeliveryPointShippingAddressRequestValidator()
    {
      RuleFor(x => x.Address).SetValidator(new DeliveryPointAddressRequestValidator());
    }
  }
} 