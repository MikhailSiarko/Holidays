using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Holidays.Storage.Entities
{
    public class CountryHolidayEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Guid CountryDateId { get; set; }

        public string Name { get; set; }

        public virtual CountryDateEntity CountryDate { get; set; }
    }
}