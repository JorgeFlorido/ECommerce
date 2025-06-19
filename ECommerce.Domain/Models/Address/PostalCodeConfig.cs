using System.Text.RegularExpressions;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models
{
    public static class PostalCodeConfig
    {
        private static readonly Dictionary<Country, (string Pattern, string Format)> PostalCodeFormats = new()
        {
            // Spain: 5 digits (e.g., 28001)
            { Country.ES, (@"^\d{5}$", "12345") },
            
            // France: 5 digits (e.g., 75012)
            { Country.FR, (@"^\d{5}$", "12345") },
            
            // Italy: 5 digits (e.g., 00144)
            { Country.IT, (@"^\d{5}$", "12345") },
            
            // Portugal: 4 digits + 3 digits (e.g., 1000-205)
            { Country.PT, (@"^\d{4}-\d{3}$", "1234-567") },
            
            // Germany: 5 digits (e.g., 10115)
            { Country.DE, (@"^\d{5}$", "12345") },
            
            // UK: Complex format (e.g., SW1A 1AA)
            { Country.GB, (@"^[A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}$", "SW1A 1AA") },
            
            // Netherlands: 4 digits + 2 letters (e.g., 1234 AB)
            { Country.NL, (@"^\d{4} ?[A-Z]{2}$", "1234 AB") }
        };

        public static bool IsValid(string? postalCode, Country country)
        {
            if (string.IsNullOrWhiteSpace(postalCode)) return false;
            if (!PostalCodeFormats.ContainsKey(country)) return false;

            var pattern = PostalCodeFormats[country].Pattern;
            return Regex.IsMatch(postalCode, pattern, RegexOptions.IgnoreCase);
        }

        public static string GetFormat(Country country)
        {
            return PostalCodeFormats.TryGetValue(country, out var format) 
                ? format.Format 
                : "Unknown format";
        }

        public static string GetPattern(Country country)
        {
            return PostalCodeFormats.TryGetValue(country, out var format)
                ? format.Pattern
                : string.Empty;
        }
    }
} 