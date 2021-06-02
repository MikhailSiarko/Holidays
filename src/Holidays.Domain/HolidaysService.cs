using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holidays.Domain.Abstracts;
using Holidays.Domain.Models;

namespace Holidays.Domain
{
    public class HolidaysService : IHolidaysService
    {
        private readonly IHolidaysClient _holidaysClient;

        private readonly IStorage<Country> _countryStorage;

        private readonly IStorage<CountryDate> _countryDatesStorage;

        private readonly IStorage<CountryHoliday> _countryHolidayStorage;

        public HolidaysService(IHolidaysClient holidaysClient,
            IStorage<Country> countryStorage,
            IStorage<CountryDate> countryDatesStorage,
            IStorage<CountryHoliday> countryHolidayStorage)
        {
            _holidaysClient = holidaysClient;
            _countryStorage = countryStorage;
            _countryDatesStorage = countryDatesStorage;
            _countryHolidayStorage = countryHolidayStorage;
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            var result = await _countryStorage.GetRangeAsync();

            var countries = result.ToList();

            if (!countries.Any())
            {
                result = await _holidaysClient.GetCountriesAsync();

                countries = result.ToList();

                await _countryStorage.SaveRangeAsync(countries);
            }

            return countries;
        }

        public async Task<IEnumerable<CountryHoliday>> GetHolidaysAsync(string countryCode, int year)
        {
            await SaveCountriesIfNotExistsAsync();

            var holidays =
                (await _countryHolidayStorage.GetRangeAsync(x => x.CountryCode == countryCode && x.Date.Year == year))
                .ToList();

            if (!holidays.Any())
            {
                holidays = await SaveHolidays(countryCode, year);
            }

            return holidays;
        }

        public async Task<int> MaxFreeDaysInRowAsync(string countryCode, int year)
        {
            await SaveCountriesIfNotExistsAsync();

            var holidays = (await _countryHolidayStorage
                    .GetRangeAsync(x => x.CountryCode == countryCode && x.Date.Year == year))
                .ToArray();

            if (!holidays.Any())
            {
                holidays = (await SaveHolidays(countryCode, year)).ToArray();
            }

            return GetMaxFreeCount(holidays, year);
        }

        public async Task<DayStatus> GetDayStatusAsync(string countryCode, DateTime date)
        {
            await SaveCountriesIfNotExistsAsync();

            var countryDate = await _countryDatesStorage.GetAsync(x => x.Country == countryCode && x.Date == date);

            if (countryDate == null)
            {
                countryDate = new CountryDate
                {
                    Date = date,
                    Country = countryCode,
                    DayStatus = await GetDayStatus(countryCode, date)
                };

                await _countryDatesStorage.SaveAsync(countryDate);
            }

            return countryDate.DayStatus;
        }

        private async Task<List<CountryHoliday>> SaveHolidays(string countryCode, int year)
        {
            var holidays = (await _holidaysClient.GetHolidaysForYearAsync(countryCode, year)).ToList();

            var countryDates = holidays.Select(x => new CountryDate
                {
                    Country = countryCode,
                    Date = x.Date,
                    DayStatus = DayStatus.Holiday
                })
                .ToList();

            await _countryDatesStorage.SaveRangeAsync(countryDates);

            holidays.ForEach(x =>
            {
                x.CountryDateId = countryDates
                    .Where(y => y.Country == x.CountryCode && y.Date == x.Date)
                    .Select(y => y.Id)
                    .Single();
            });

            await _countryHolidayStorage.SaveRangeAsync(holidays);

            return holidays;
        }

        private async Task SaveCountriesIfNotExistsAsync()
        {
            var result = await _countryStorage.GetRangeAsync();

            var countries = result.ToList();

            if (!countries.Any())
            {
                result = await _holidaysClient.GetCountriesAsync();

                countries = result.ToList();

                await _countryStorage.SaveRangeAsync(countries);
            }
        }

        private async Task<DayStatus> GetDayStatus(string country, DateTime date)
        {
            var isWorkDay = await _holidaysClient.IsWorkDayAsync(country, date);

            if (isWorkDay)
                return DayStatus.Workday;

            var isPublicHoliday = await _holidaysClient.IsPublicHolidayAsync(country, date);

            if (isPublicHoliday)
                return DayStatus.Holiday;

            return DayStatus.FreeDay;
        }

        private int GetMaxFreeCount(CountryHoliday[] holidays, int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var lasDay = new DateTime(year, 12, 31);

            var totalDays = Convert.ToInt32(lasDay.Subtract(firstDay).TotalDays);

            var calendar = Enumerable
                .Range(0, totalDays + 1)
                .Select(i => firstDay.AddDays(i))
                .ToArray();

            return GetMaxCount(calendar, holidays);
        }

        private int GetMaxCount(DateTime[] days, CountryHoliday[] holidays)
        {
            var count = 0;
            var result = 0;

            foreach (var day in days)
            {
                if (!IsHoliday(day, holidays) && !IsWeekend(day))
                    count = 0;
                else
                {
                    count++;
                    result = Math.Max(result, count);
                }
            }

            return result;
        }

        private bool IsHoliday(DateTime date, IEnumerable<CountryHoliday> holidays) =>
            holidays.Any(x => x.Date == date);

        private bool IsWeekend(DateTime date) =>
            date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
}