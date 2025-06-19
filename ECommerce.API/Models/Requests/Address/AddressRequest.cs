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

  public class CustomerAddressRequest : AddressRequest
  {
    public bool IsPrimary { get; set; }
  }

  // Request classes for order shipping addresses (not for CRUD operations)
  public class DeliveryPointAddressRequest : AddressRequest
  {
    [Required]
    public string ShopName { get; set; } = null!;
    
    [Required]
    public string ContactNumber { get; set; } = null!;
  }

  public class LockerAddressRequest : AddressRequest
  {
    [Required]
    public string LockerId { get; set; } = null!;
    
    [Required]
    public string Provider { get; set; } = null!;
  }

  // Discriminated union for shipping addresses
  public abstract class OrderShippingAddressRequest
  {
    public abstract AddressType Type { get; }
  }

  public class CustomerShippingAddressRequest : OrderShippingAddressRequest
  {
    public override AddressType Type => AddressType.CustomerAddress;
    
    [Required]
    public CustomerAddressRequest Address { get; set; } = null!;
  }

  public class DeliveryPointShippingAddressRequest : OrderShippingAddressRequest
  {
    public override AddressType Type => AddressType.DeliveryPoint;
    
    [Required]
    public DeliveryPointAddressRequest Address { get; set; } = null!;
  }

  public class LockerShippingAddressRequest : OrderShippingAddressRequest
  {
    public override AddressType Type => AddressType.Locker;
    
    [Required]
    public LockerAddressRequest Address { get; set; } = null!;
  }

  public class OrderBillingAddressRequest
  {
    [Required]
    public CustomerAddressRequest CustomerAddress { get; set; } = null!;
  }
} 