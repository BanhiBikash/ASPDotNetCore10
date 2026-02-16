using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Weather_App.Models;

namespace Weather_App.Controllers
{
    public class HomeController : Controller
    {
        List<City> cities = new List<City>()
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
            return View(cities);
        }

        [Route("/weather/{cityCode:required}")]
        public IActionResult SpecificCity()
        {
            City? data = cities.FirstOrDefault(c =>
           c.CityUniqueCode.Equals(Convert.ToString(Request.RouteValues["cityCode"]), StringComparison.OrdinalIgnoreCase));

            if (data != null)
            {
                return View(data);
            }
            else
            {
                return NotFound("City not found.");
            }
        }
    }
}
