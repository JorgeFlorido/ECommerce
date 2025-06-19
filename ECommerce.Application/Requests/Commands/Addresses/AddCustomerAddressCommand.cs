using ECommerce.Domain.Models;
using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class AddCustomerAddressCommand : IRequest<CustomerAddress>
  {
    public Guid CustomerId { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public Country Country { get; set; }
    public bool IsPrimary { get; set; }
  }
} 