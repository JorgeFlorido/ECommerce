using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
    // Used only for shipping addresses, not for customer address CRUD
    public class DeliveryPointAddressRequest : AddressRequest
    {
        [Required]
        public string ShopName { get; set; } = null!;
        
        [Required]
        public string ContactNumber { get; set; } = null!;
    }
} 