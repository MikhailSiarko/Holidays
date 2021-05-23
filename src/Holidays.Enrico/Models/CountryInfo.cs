using System.Text.Json.Serialization;

namespace Holidays.Enrico.Models
{
    public class CountryInfo
    {
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
        
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    }
}