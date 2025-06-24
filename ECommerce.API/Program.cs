using ECommerce.Infrastructure.Extensions;
using ECommerce.Application.Extensions;
using ECommerce.API.Extensions;
using ECommerce.Application.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
      builder.Services.AddServices();
      builder.Services.AddApplication();
      builder.Services.AddFactories();

      builder.Services.AddValidators();
      builder.Services.AddMappers();

      builder.Services.AddAuthentication(options =>
      {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = builder.Configuration["Jwt:Issuer"],
              ValidAudience = builder.Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters"))
          };
      });

      builder.Services.AddAuthorization(options =>
      {
          options.AddPolicy("OrderManagement", policy =>
              policy.RequireRole(AppRoles.Admin));

          options.AddPolicy("CustomerData", policy =>
              policy.RequireAssertion(context =>
                  context.User.IsInRole(AppRoles.Admin) ||
                  context.User.FindFirst("sub")?.Value == context.Resource?.ToString()));
      });

      var app = builder.Build();

      app.UseMiddleware<ExceptionMiddleware>();

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllers();

      app.Run();
    }
  }
}
