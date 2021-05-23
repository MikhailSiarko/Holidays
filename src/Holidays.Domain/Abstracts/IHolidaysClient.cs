using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Holidays.Domain.Models;

namespace Holidays.Domain.Abstracts
{
    public interface IHolidaysClient
    {
        Task<IEnumerable<Country>> GetCountriesAsync();

        Task<IEnumerable<CountryHoliday>> GetHolidaysForYearAsync(string country, int year);

        Task<bool> IsPublicHolidayAsync(string country, DateTime date);

        Task<bool> IsWorkDayAsync(string country, DateTime date);
    }
}