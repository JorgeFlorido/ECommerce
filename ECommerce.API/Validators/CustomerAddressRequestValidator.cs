using ECommerce.API.Models.Requests.Address;
using ECommerce.Domain.Models;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class CustomerAddressRequestValidator : AbstractValidator<CustomerAddressRequest>
  {
    public CustomerAddressRequestValidator()
    {
      RuleFor(x => x.Street).NotEmpty();
      RuleFor(x => x.City).NotEmpty();
      RuleFor(x => x.State).NotEmpty();
      RuleFor(x => x.PostalCode)
        .NotEmpty()
        .Must((request, postalCode) => PostalCodeConfig.IsValid(postalCode, request.Country))
        .WithMessage((request, _) => $"Invalid postal code format for {request.Country}. Expected format: {PostalCodeConfig.GetFormat(request.Country)}");
      RuleFor(x => x.Country).IsInEnum();
    }
  }
} 