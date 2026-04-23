using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.web.DBcontext;
using CitiesManager.web.Models;
using Asp.Versioning;
using CitiesManager.web.Controllers;

namespace CitiesManager.web.Controllers.v2
{
    //[Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CitiesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        /// <summary>
        /// Retrieves all cities from the data store.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/>
        /// with a collection of <see cref="City"/> objects representing all available cities.</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<string>>> GetCities()
        {
            List<City> CityList = await _context.Cities.ToListAsync();

            if(CityList == null || CityList.Count == 0)
            {
                return Problem(detail:"No cities found!", statusCode:400, title:"City search.");
            }

            List<string>? CityNameList = new List<string>();

            foreach(City city in CityList)
            {
                CityNameList.Add(city.CityName);
            }

            return CityNameList;
        }
    }
}
