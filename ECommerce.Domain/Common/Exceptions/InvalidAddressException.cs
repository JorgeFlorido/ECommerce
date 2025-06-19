using System;

namespace ECommerce.Domain.Common.Exceptions
{
    public class InvalidAddressException : DomainException
    {
        public InvalidAddressException(string message) : base(message)
        {
        }

        public InvalidAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 