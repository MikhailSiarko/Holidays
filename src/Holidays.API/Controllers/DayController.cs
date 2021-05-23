using System;
using System.Threading.Tasks;
using Holidays.Domain.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Holidays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DayController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;

        public DayController(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        [HttpGet("{day:int}-{month:int}-{year:int}/{country}/status")]
        public async Task<IActionResult> Get(int day, int month, int year, string country)
        {
            var dayStatus = await _holidaysService.GetDayStatusAsync(country, new DateTime(year, month, day));
            
            return Ok(dayStatus.ToString("G"));
        }
    }
}