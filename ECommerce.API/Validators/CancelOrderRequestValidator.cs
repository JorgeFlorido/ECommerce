using ECommerce.API.Models.Requests.Order;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
  {
    public CancelOrderRequestValidator()
    {
      RuleFor(x => x.Reason).NotEmpty().MinimumLength(5);
    }
  }
} 