using System;

namespace Holidays.Domain.Models
{
    public class CountryHoliday
    {
        public Guid Id { get; set; }

        public Guid CountryDateId { get; set; }
        
        public DateTime Date { get; set; }

        public string CountryCode { get; set; }

        public string Name { get; set; }
    }
}