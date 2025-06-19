using System;

namespace ECommerce.Domain.Common.Exceptions
{
    public class PaymentException : DomainException
    {
        public PaymentException(string message) : base(message)
        {
        }

        public PaymentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
