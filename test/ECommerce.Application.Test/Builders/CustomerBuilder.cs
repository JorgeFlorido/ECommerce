using ECommerce.Domain.Models;
using ECommerce.Domain.Models.User;
using System.Reflection;

namespace ECommerce.Application.Test.Builders
{
  public class CustomerBuilder
  {
    private readonly Customer _customer;

    public CustomerBuilder()
    {
      _customer = (Customer)Activator.CreateInstance(typeof(Customer), true)!;
    }

    public CustomerBuilder WithId(Guid id)
    {
      var prop = typeof(Customer).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
      if (prop != null && prop.CanWrite)
        prop.SetValue(_customer, id);
      else
      {
        var field = typeof(Customer).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        field?.SetValue(_customer, id);
      }
      return this;
    }

    public CustomerBuilder WithPhoneNumber(string? phoneNumber)
    {
      _customer.PhoneNumber = phoneNumber;
      return this;
    }

    public CustomerBuilder WithAddresses(List<CustomerAddress> addresses)
    {
      _customer.Addresses = addresses;
      return this;
    }

    public Customer Build() => _customer;
  }
}