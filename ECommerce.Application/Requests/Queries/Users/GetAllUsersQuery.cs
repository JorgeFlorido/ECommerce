using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Users
{
  public class GetAllUsersQuery : IRequest<IEnumerable<Customer>>
  {
  }
}
