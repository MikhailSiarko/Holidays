using System.Threading.Tasks;
using Holidays.Domain.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Holidays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CountriesController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;

        public CountriesController(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var countries = await _holidaysService.GetCountriesAsync();

            return Ok(countries);
        }
    }
}