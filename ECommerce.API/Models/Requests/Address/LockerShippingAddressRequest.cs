using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.API.Models.Requests.Address
{
    public class LockerShippingAddressRequest : OrderShippingAddressRequest
    {
        public override AddressType Type => AddressType.Locker;
        
        [Required]
        public LockerAddressRequest Address { get; set; } = null!;
    }
} 