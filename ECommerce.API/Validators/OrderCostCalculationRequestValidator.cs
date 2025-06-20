using ECommerce.API.Models.Requests.Order;
using FluentValidation;

namespace ECommerce.API.Validators
{
    public class OrderCostCalculationRequestValidator : AbstractValidator<OrderCostCalculationRequest>
    {
        public OrderCostCalculationRequestValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Items).NotNull().NotEmpty();
            RuleForEach(x => x.Items).NotNull();
            RuleFor(x => x.ShippingAddress).NotNull();
            RuleFor(x => x.BillingAddress).NotNull();
        }
    }
} 