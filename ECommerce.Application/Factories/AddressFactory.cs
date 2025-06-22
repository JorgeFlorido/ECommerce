using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Factories
{
  internal class AddressFactory : IAddressFactory
  {
    public OrderShippingAddress? CreateShippingAddress(OrderShippingAddressCommand? command, Guid customerId)
    {
      if (command == null) return null;

      Address address = command switch
      {
        CustomerShippingAddressCommand customerCommand => MapToCustomerAddress(customerCommand.Address, customerId),
        DeliveryPointShippingAddressCommand deliveryCommand => MapToDeliveryPointAddress(deliveryCommand.Address),
        LockerShippingAddressCommand lockerCommand => MapToLockerAddress(lockerCommand.Address),
        _ => throw new ArgumentException($"Unsupported address type: {command.GetType().Name}")
      };

      return new OrderShippingAddress
      {
        Id = Guid.NewGuid(),
        Type = command.Type,
        Address = address
      };
    }

    public OrderBillingAddress? CreateBillingAddress(OrderBillingAddressCommand? command, Guid customerId)
    {
      if (command == null) return null;

      return new OrderBillingAddress
      {
        Id = Guid.NewGuid(),
        CustomerAddress = new CustomerAddress
        {
          Id = Guid.NewGuid(),
          CustomerId = customerId,
          Street = command.CustomerAddress.Street,
          City = command.CustomerAddress.City,
          State = command.CustomerAddress.State,
          PostalCode = new PostalCode(command.CustomerAddress.PostalCode, command.CustomerAddress.Country),
          Country = command.CustomerAddress.Country,
          IsPrimary = command.CustomerAddress.IsPrimary
        }
      };
    }

    private static CustomerAddress MapToCustomerAddress(CustomerAddressCommand command, Guid customerId)
    {
      return new CustomerAddress
      {
        Id = Guid.NewGuid(),
        CustomerId = customerId,
        Street = command.Street,
        City = command.City,
        State = command.State,
        PostalCode = new PostalCode(command.PostalCode, command.Country),
        Country = command.Country,
        IsPrimary = command.IsPrimary
      };
    }

    private static DeliveryPointAddress MapToDeliveryPointAddress(DeliveryPointAddressCommand command)
    {
      return new DeliveryPointAddress
      {
        Id = Guid.NewGuid(),
        Street = command.Street,
        City = command.City,
        State = command.State,
        PostalCode = new PostalCode(command.PostalCode, command.Country),
        Country = command.Country,
        ShopName = command.ShopName,
        ContactNumber = command.ContactNumber
      };
    }

    private static LockerAddress MapToLockerAddress(LockerAddressCommand command)
    {
      return new LockerAddress
      {
        Id = Guid.NewGuid(),
        Street = command.Street,
        City = command.City,
        State = command.State,
        PostalCode = new PostalCode(command.PostalCode, command.Country),
        Country = command.Country,
        LockerId = command.LockerId,
        Provider = command.Provider
      };
    }
  }
}
