using System.Collections.Generic;

namespace Holidays.API.Models
{
    public class HolidaysGroupedByMonth
    {
        public string CountryCode { get; set; }

        public IEnumerable<HolidayGroupItem> HolidaysByMonth { get; set; }
    }
}