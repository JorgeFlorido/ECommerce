using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
    public class OrderBillingAddressRequest
    {
        [Required]
        public CustomerAddressRequest CustomerAddress { get; set; } = null!;
    }
} 