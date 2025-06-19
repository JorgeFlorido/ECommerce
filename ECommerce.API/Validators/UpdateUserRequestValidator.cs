using ECommerce.API.Models.Requests.User;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
  {
    public UpdateUserRequestValidator()
    {
      RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
      RuleFor(x => x.Name).NotEmpty().When(x => x.Name != null);
      RuleFor(x => x.Surname).NotEmpty().When(x => x.Surname != null);
      RuleFor(x => x.PhoneNumber).Matches(@"^\+?[0-9]{7,15}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
  }
} 