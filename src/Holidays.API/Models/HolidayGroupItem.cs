using System.Collections.Generic;

namespace Holidays.API.Models
{
    public class HolidayGroupItem
    {
        public int Month { get; set; }

        public IEnumerable<CountryHolidayItem> Holidays { get; set; }
    }
}