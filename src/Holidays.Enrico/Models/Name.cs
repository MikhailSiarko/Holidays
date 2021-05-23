using System.Text.Json.Serialization;

namespace Holidays.Enrico.Models
{
    public class NameInfo
    {
        [JsonPropertyName("lang")]
        public string Lang { get; set; }
        
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}