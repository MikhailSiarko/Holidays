using System.Text.Json.Serialization;

namespace Holidays.Enrico.Models
{
    public class IsPublicHolidayInfo
    {
        [JsonPropertyName("isPublicHoliday")]
        public bool IsPublicHoliday { get; set; }
    }
}