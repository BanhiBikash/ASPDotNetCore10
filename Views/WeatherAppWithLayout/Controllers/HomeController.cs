using Microsoft.AspNetCore.Mvc;
using WeatherAppWithLayout.Models;

namespace Layout_Views1.Controllers
{
    public class HomeController : Controller
    {
        List<City> Cities = new List<City>()
        {

            new City{
                CityUniqueCode = "LDN",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 08:00"),
                TemperatureFahrenheit = 33
            },

            new City{
                CityUniqueCode = "NYC",
                CityName = "New York",
                DateAndTime = Convert.ToDateTime("2030-01-01 03:00"),
                TemperatureFahrenheit = 60
            },

            new City{
                CityUniqueCode = "PAR",
                CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 09:00"),
                TemperatureFahrenheit = 82
            }

        };
        [Route("/")]
        public IActionResult Index()
        {
            return View(Cities);
        }

        [Route("/CityPage/{cityCode?}")]
        public IActionResult CityPage(string? cityCode)
        {
            City? Data = Cities.Where(c => c.CityUniqueCode == cityCode).FirstOrDefault();

            if (Data != null)
            {
                return View(Data);
            }
            else
            {
                return NotFound("The specific city data is not found.");
            }
        }
    }
}
