using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Holidays.Domain.Models;

namespace Holidays.Domain.Abstracts
{
    public interface IHolidaysService
    {
        Task<IEnumerable<Country>> GetCountriesAsync();

        Task<IEnumerable<CountryHoliday>> GetHolidaysAsync(string country, int year);

        Task<int> MaxFreeDaysInRowAsync(string country, int year);

        Task<DayStatus> GetDayStatusAsync(string country, DateTime date);
    }
}