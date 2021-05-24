using System;

namespace Holidays.Domain.Models
{
    public class CountryDate
    {
        public Guid Id { get; set; }

        public string Country { get; set; }

        public DateTime Date { get; set; }

        public DayStatus DayStatus { get; set; }

        public bool IsFree =>
            Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday ||
            DayStatus is DayStatus.FreeDay or DayStatus.Holiday;
    }
}