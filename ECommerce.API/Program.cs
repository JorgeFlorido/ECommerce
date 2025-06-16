using ECommerce.Infrastructure.Extensions;
using ECommerce.Application.Extensions;
using ECommerce.API.Extensions;

namespace ECommerce.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllers();
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      builder.Services.AddInfrastructure(builder.Configuration);
      builder.Services.AddRepositories();
      builder.Services.AddApplication();

      builder.Services.AddValidators();
      builder.Services.AddMappers();

      var app = builder.Build();

      app.UseMiddleware<ExceptionMiddleware>();

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();

      app.MapControllers();

      app.Run();
    }
  }
}
