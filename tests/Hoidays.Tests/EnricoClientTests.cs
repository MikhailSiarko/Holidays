using System;
using System.Linq;
using System.Threading.Tasks;
using Holidays.Enrico;
using Xunit;

namespace Hoidays.Tests
{
    public class EnricoClientFixture : IDisposable
    {
        public EnricoClient EnricoClient { get; }

        public EnricoClientFixture()
        {
            EnricoClient = new EnricoClient();
        }

        public void Dispose()
        {
            EnricoClient?.Dispose();
        }
    }
    
    public class EnricoClientTests : IClassFixture<EnricoClientFixture>
    {
        private readonly EnricoClientFixture _enricoClientFixture;

        public EnricoClientTests(EnricoClientFixture enricoClientFixture)
        {
            _enricoClientFixture = enricoClientFixture;
        }

        [Fact]
        public async Task GetHolidaysForMonthAsync_Takes_DEU_2022_1_ResultIsNotNullOrEmpty()
        {
            var result = await _enricoClientFixture.EnricoClient.GetHolidaysForYearAsync("deu", 2022);

            var holidays = result.ToArray();
            
            Assert.NotNull(holidays);
            Assert.NotEmpty(holidays);
            Assert.All(holidays, c =>
            {
                Assert.NotNull(c.Name);
            });
        }

        [Fact]
        public async Task IsPublicHolidayAsync_Takes_ABC_2022_2_1_Returns_False()
        {
            var result = await _enricoClientFixture.EnricoClient.IsPublicHolidayAsync("abc", new DateTime(2022, 2, 1));
            
            Assert.False(result);
        }
        
        [Fact]
        public async Task IsPublicHolidayAsync_Takes_DEU_2022_1_1_Returns_True()
        {
            var result = await _enricoClientFixture.EnricoClient.IsPublicHolidayAsync("deu", new DateTime(2022, 1, 1));
            
            Assert.True(result);
        }
        
        [Fact]
        public async Task IsPublicHolidayAsync_Takes_DEU_2022_2_1_Returns_False()
        {
            var result = await _enricoClientFixture.EnricoClient.IsPublicHolidayAsync("deu", new DateTime(2022, 2, 1));
            
            Assert.False(result);
        }

        [Fact]
        public async Task IsWorkDayAsync_Takes_ABC_2022_2_1_Returns_False()
        {
            var result = await _enricoClientFixture.EnricoClient.IsWorkDayAsync("abc", new DateTime(2022, 1, 1));
            
            Assert.False(result);
        }
        
        [Fact]
        public async Task IsWorkDayAsync_Takes_DEU_2022_2_1_Returns_True()
        {
            var result = await _enricoClientFixture.EnricoClient.IsWorkDayAsync("deu", new DateTime(2022, 2, 1));
            
            Assert.True(result);
        }
        
        [Fact]
        public async Task IsWorkDayAsync_Takes_DEU_2022_2_1_Returns_False()
        {
            var result = await _enricoClientFixture.EnricoClient.IsWorkDayAsync("deu", new DateTime(2022, 1, 1));
            
            Assert.False(result);
        }

        [Fact]
        public async Task GetCountriesAsync_ResultIsNotNullOrEmpty()
        {
            var result = await _enricoClientFixture.EnricoClient.GetCountriesAsync();

            var countries = result.ToArray();

            Assert.NotNull(countries);
            Assert.NotEmpty(countries);
            Assert.All(countries, c =>
            {
                Assert.NotNull(c.CountryCode);
                Assert.NotNull(c.FullName);
            });
        }
    }
}