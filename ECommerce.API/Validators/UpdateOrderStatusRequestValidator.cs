using ECommerce.API.Models.Requests.Order;
using FluentValidation;

namespace ECommerce.API.Validators
{
  public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
  {
    public UpdateOrderStatusRequestValidator()
    {
      RuleFor(x => x.Status).IsInEnum();
      RuleFor(x => x.Notes).MaximumLength(500);
    }
  }
} 