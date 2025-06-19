using System;

namespace ECommerce.Domain.Common.Exceptions
{
    public class ProductException : DomainException
    {
        public ProductException(string message) : base(message)
        {
        }

        public ProductException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 