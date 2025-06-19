using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class DeliveryPointAddressRequestValidator : AbstractValidator<DeliveryPointAddressRequest>
  {
    public DeliveryPointAddressRequestValidator()
    {
      RuleFor(x => x.Street).NotEmpty();
      RuleFor(x => x.City).NotEmpty();
      RuleFor(x => x.State).NotEmpty();
      RuleFor(x => x.PostalCode).NotEmpty();
      RuleFor(x => x.Country).NotEmpty();
      RuleFor(x => x.ShopName).NotEmpty();
      RuleFor(x => x.ContactNumber).NotEmpty();
    }
  }
} 