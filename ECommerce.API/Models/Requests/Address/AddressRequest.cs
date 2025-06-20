using ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Address
{
  public class AddressRequest
  {
    [Required]
    public string Street { get; set; } = null!;
    
    [Required]
    public string City { get; set; } = null!;
    
    [Required]
    public string State { get; set; } = null!;
    
    [Required]
    public string PostalCode { get; set; } = null!;
    
    [Required]
    public Country Country { get; set; }
  }
} 