using System;

namespace ECommerce.Domain.Common.Exceptions
{
    public class CustomerException : DomainException
    {
        public CustomerException(string message) : base(message)
        {
        }

        public CustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
