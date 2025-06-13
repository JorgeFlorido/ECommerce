using ECommerce.API.Mappers;
using ECommerce.API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ECommerce.API.Extensions
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
      services.AddValidatorsFromAssemblyContaining<AddProductRequestValidator>();
      services.AddFluentValidationAutoValidation();

      return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
      services.AddAutoMapper(typeof(ProductMappingProfile));
      return services;
    }
  }
}
