using System.Linq;
using System.Threading.Tasks;
using Holidays.API.Models;
using Holidays.Domain.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Holidays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{year:int}/{country}")]
    [Produces("application/json")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;

        public HolidaysController(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int year, string country)
        {
            var holidays = await _holidaysService.GetHolidaysAsync(country, year);
            
            var grouped = new HolidaysGroupedByMonth
            {
                CountryCode = country,
                HolidaysByMonth = holidays
                    .GroupBy(x => x.Date.Month)
                    .Select(x => new HolidayGroupItem
                    {
                        Month = x.Key,
                        Holidays = x.Select(y => new CountryHolidayItem 
                        {
                            Date = y.Date.ToShortDateString(),
                            Name = y.Name
                        })
                    })
                    .ToList()
            };
            
            return Ok(grouped);
        }
        
        [HttpGet("max-free-days-in-row")]
        public async Task<IActionResult> MaxFreeDaysInRow(int year, string country)
        {
            var count = await _holidaysService.MaxFreeDaysInRowAsync(country, year);
            return Ok(new {maxFreeDaysInRow = count});
        }
    }
}