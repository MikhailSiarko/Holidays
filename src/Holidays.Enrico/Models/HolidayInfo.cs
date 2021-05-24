using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Holidays.Enrico.Models
{
    public class HolidayInfo
    {
        public HolidayInfo()
        {
            Names = new List<NameInfo>();
        }
        
        [JsonPropertyName("date")]
        public DateInfo Date { get; set; }

        [JsonPropertyName("name")]
        public IEnumerable<NameInfo> Names { get; set; }
    }
}