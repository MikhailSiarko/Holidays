using System.Text.Json.Serialization;

namespace Holidays.Enrico.Models
{
    public class IsWorkDayInfo
    {
        [JsonPropertyName("isWorkDay")]
        public bool IsWorkDay { get; set; }
    }
}