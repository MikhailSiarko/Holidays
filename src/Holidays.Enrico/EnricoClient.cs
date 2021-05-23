using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Holidays.Domain.Abstracts;
using Holidays.Domain.Models;
using Holidays.Enrico.Models;

namespace Holidays.Enrico
{
    public class EnricoClient : IHolidaysClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        
        private const string BaseAddress = "https://kayaposoft.com/enrico/json/v2.0";

        public EnricoClient()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri(BaseAddress)};
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await Try<IEnumerable<CountryInfo>, IEnumerable<Country>>(
                "?action=getSupportedCountries",
                res => res.Select(x => new Country
                {
                    CountryCode = x.CountryCode,
                    FullName = x.FullName
                }));
        }
        
        public async Task<IEnumerable<CountryHoliday>> GetHolidaysForYearAsync(string country, int year)
        {
            return await Try<IEnumerable<HolidayInfo>, IEnumerable<CountryHoliday>>(
                $"?action=getHolidaysForYear&&year={year}&country={country}&holidayType=public_holiday",
                res => res.Select(x => new CountryHoliday
                {
                    CountryCode = country,
                    Date = new DateTime(x.Date.Year, x.Date.Month, x.Date.Day),
                    Name = x.Names.Where(y => y.Lang == "en").Select(y => y.Text).FirstOrDefault()
                }));
        }
        
        public async Task<bool> IsPublicHolidayAsync(string country, DateTime date)
        {
            return await Try<IsPublicHolidayInfo, bool>(
                $"?action=isPublicHoliday&&date={date.Day}-{date.Month}-{date.Year}&country={country}",
                res => res.IsPublicHoliday);
        }
        
        public async Task<bool> IsWorkDayAsync(string country, DateTime date)
        {
            return await Try<IsWorkDayInfo, bool>(
                $"?action=isWorkDay&&date={date.Day}-{date.Month}-{date.Year}&country={country}",
                res => res.IsWorkDay);
        }

        private async Task<TV> Try<TK, TV>(string query, Func<TK, TV> func)
        {
            var response = await _httpClient.GetAsync(query);

            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<TK>(content);

            return func(result);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}