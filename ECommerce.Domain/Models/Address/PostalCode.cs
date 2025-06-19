using System;
using ECommerce.Domain.Common.Exceptions;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models
{
    public sealed class PostalCode : IEquatable<PostalCode>
    {
        public string Value { get; }
        public Country Country { get; }

        public PostalCode(string value, Country country)
        {
            if (!Models.PostalCodeConfig.IsValid(value, country))
                throw new ValidationException($"Invalid postal code '{value}' for country '{country}'.");
            Value = value;
            Country = country;
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) => Equals(obj as PostalCode);
        public bool Equals(PostalCode? other) => other is not null && Value == other.Value && Country == other.Country;
        public override int GetHashCode() => HashCode.Combine(Value, Country);
    }
} 