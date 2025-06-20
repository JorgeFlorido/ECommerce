using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.API.Models.Requests.Address
{
    public class DeliveryPointShippingAddressRequest : OrderShippingAddressRequest
    {
        public override AddressType Type => AddressType.DeliveryPoint;
        
        [Required]
        public DeliveryPointAddressRequest Address { get; set; } = null!;
    }
} 