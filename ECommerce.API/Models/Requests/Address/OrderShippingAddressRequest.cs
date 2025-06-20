using ECommerce.Domain.Enums;

namespace ECommerce.API.Models.Requests.Address
{
    public abstract class OrderShippingAddressRequest
    {
        public abstract AddressType Type { get; }
    }
} 