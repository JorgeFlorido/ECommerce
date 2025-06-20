using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
    public class CustomerAddressRequest : AddressRequest
    {
        public bool IsPrimary { get; set; }
    }
} 