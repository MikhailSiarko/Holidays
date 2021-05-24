using System.Collections.Generic;

namespace Holidays.Storage.Entities
{
    public class CountryEntity
    {
        public string Code { get; set; }

        public string FullName { get; set; }

        public virtual ICollection<CountryDateEntity> Days { get; set; }
    }
}