using ECommerce.API.Models.Requests.User;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
  {
    public AddUserRequestValidator()
    {
      RuleFor(x => x.Email).NotEmpty().EmailAddress();
      RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
      RuleFor(x => x.Name).NotEmpty();
      RuleFor(x => x.Surname).NotEmpty();
      RuleFor(x => x.PhoneNumber).Matches(@"^\+?[0-9]{7,15}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
  }
} 