using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.API.Models.Requests.Address
{
    public class CustomerShippingAddressRequest : OrderShippingAddressRequest
    {
        public override AddressType Type => AddressType.CustomerAddress;
        
        [Required]
        public CustomerAddressRequest Address { get; set; } = null!;
    }
} 