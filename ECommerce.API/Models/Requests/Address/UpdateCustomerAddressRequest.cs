using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
    public class UpdateCustomerAddressRequest : CustomerAddressRequest
    {
        [Required]
        public Guid AddressId { get; set; }
    }
} 