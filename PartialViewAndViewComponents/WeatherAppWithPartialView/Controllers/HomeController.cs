using Microsoft.AspNetCore.Mvc;
using WeatherAppWithPartialView.Models;

namespace WeatherAppWithPartialView.Controllers
{
    public class HomeController : Controller
    {
        List<City> CityData = new List<City>()
        {
       new City{ CityUniqueCode = "LDN",
        CityName = "London",
        DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
        TemperatureFahrenheit = 33 },

         new City{ CityUniqueCode = "NYC",
        CityName = "London",
        DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
        TemperatureFahrenheit = 60 },

          new City{
        CityUniqueCode = "PAR",
        CityName = "Paris",
        DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
        TemperatureFahrenheit = 82 }
        };


        [Route("/")]
        public IActionResult Index()
        {
            return View(CityData);
        }

        [Route("/CityPage/{CityCode}")]
        public IActionResult CityPage(string? CityCode)
        {
            City? Data = CityData.Where(x => x.CityUniqueCode == CityCode).FirstOrDefault();

            if (Data != null)
            {
                return View(Data);
            }
            else
            {
                return NotFound("Data for this city code not available.");
            }
        }

        [Route("/CityData/{CityCode}")]
        public IActionResult CityInfo(string? CityCode)
        {

            City? Data = CityData.Where(x => x.CityUniqueCode == CityCode).FirstOrDefault();

            if (Data != null)
            {
                return PartialView("_Partial", Data);
            }
            else
            {
                return NotFound("Data for this city code not available.");
            }
        }
    }
}
