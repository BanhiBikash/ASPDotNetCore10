using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.web.DBcontext;
using CitiesManager.web.Models;
using Asp.Versioning;

namespace CitiesManager.web.Controllers.v1
{
    [ApiVersion("1.0")]
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
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        // GET: api/Cities/5
        /// <summary>
        /// Retrieves the city with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the city to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="ActionResult{City}"/> representing the city with the specified identifier if found; otherwise, a
        /// problem response indicating that the city was not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return Problem(detail:"City not found!", statusCode:400, title:"City ID search.");
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates the details of an existing city with the specified identifier.
        /// </summary>
        /// <remarks>If the specified city does not exist, the method returns a 404 Not Found response. If
        /// the <paramref name="id"/> does not match the <c>CityId</c> of the provided <paramref name="city"/>, the
        /// method returns a 400 Bad Request response. Concurrency conflicts during the update will result in an
        /// exception unless the city no longer exists.</remarks>
        /// <param name="id">The unique identifier of the city to update.</param>
        /// <param name="city">The updated city data. The <see cref="City.CityId"/> property must match the <paramref name="id"/>
        /// parameter.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see cref="NoContentResult"/>
        /// if the update is successful; <see cref="NotFoundResult"/> if the city does not exist; or <see
        /// cref="BadRequestResult"/> if the identifiers do not match.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, City city)
        {
            if (id != city.CityId)
            {
                return BadRequest();
            }

            var cityEntry = await _context.Cities.FindAsync(city);

            if(cityEntry == null)   //npt found
            {
                return NotFound();
            }
            else   //found
            {
                //updting values
                cityEntry.CityName = city.CityName;
            }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            return NoContent();
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new city and adds it to the data store.
        /// </summary>
        /// <remarks>If the operation is successful, the response has HTTP status code 201 (Created) and
        /// includes the created city in the response body. The location header points to the endpoint for retrieving
        /// the newly created city.</remarks>
        /// <param name="city">The city entity to add. Must not be null.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the created city and a location header with a URI to retrieve
        /// the city by its identifier.</returns>
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.CityId }, city);
        }

        // DELETE: api/Cities/5
        /// <summary>
        /// Deletes the city with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the city to delete.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the delete operation. Returns <see
        /// cref="NotFoundResult"/> if the city does not exist; otherwise, returns <see cref="NoContentResult"/> on
        /// successful deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.CityId == id);
        }
    }
}
