using System;

namespace ECommerce.Domain.Common.Exceptions
{
    public class OrderException : DomainException
    {
        public OrderException(string message) : base(message)
        {
        }

        public OrderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 