using ECommerce.API.Models.Requests.Address;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class LockerAddressRequestValidator : AbstractValidator<LockerAddressRequest>
  {
    public LockerAddressRequestValidator()
    {
      RuleFor(x => x.Street).NotEmpty();
      RuleFor(x => x.City).NotEmpty();
      RuleFor(x => x.State).NotEmpty();
      RuleFor(x => x.PostalCode).NotEmpty();
      RuleFor(x => x.Country).NotEmpty();
      RuleFor(x => x.LockerId).NotEmpty();
      RuleFor(x => x.Provider).NotEmpty();
    }
  }
} 