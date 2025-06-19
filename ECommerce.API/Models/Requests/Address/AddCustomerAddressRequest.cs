using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
    public class AddCustomerAddressRequest : CustomerAddressRequest
    {
        [Required]
        public Guid CustomerId { get; set; }
    }
} 