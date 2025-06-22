using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces
{
  internal interface IAddressFactory
  {
    OrderShippingAddress? CreateShippingAddress(OrderShippingAddressCommand? command, Guid customerId);
    OrderBillingAddress? CreateBillingAddress(OrderBillingAddressCommand? command, Guid customerId);
  }
}
