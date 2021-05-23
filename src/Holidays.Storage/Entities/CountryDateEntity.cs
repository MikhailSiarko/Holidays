using System;
using System.ComponentModel.DataAnnotations.Schema;
using Holidays.Domain.Models;

namespace Holidays.Storage.Entities
{
    public class CountryDateEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }
        
        public string CountryCode { get; set; }
        
        public DayStatus Status { get; set; }

        public virtual CountryEntity Country { get; set; }

        public virtual CountryHolidayEntity Holiday { get; set; }
    }
}