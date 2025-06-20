using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
    // Used only for shipping addresses, not for customer address CRUD
    public class LockerAddressRequest : AddressRequest
    {
        [Required]
        public string LockerId { get; set; } = null!;
        
        [Required]
        public string Provider { get; set; } = null!;
    }
} 