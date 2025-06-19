using System;

namespace ECommerce.Domain.Common.Exceptions
{
    public class InventoryException : DomainException
    {
        public InventoryException(string message) : base(message)
        {
        }

        public InventoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
