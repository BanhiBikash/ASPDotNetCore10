using Microsoft.AspNetCore.Mvc;
using ViewComponent1.Models;

namespace ViewComponent1.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/LoadIndianCities")]
        public IActionResult IndianCity()
        {
            List<City> Cities = new List<City>()
        {
            new City(){CityUniqueCode="MUB",CityName="Mumbai",DateAndTime=DateTime.Now,TemperatureFahrenheit=75},
            new City(){CityUniqueCode="DEL",CityName="Delhi",DateAndTime=DateTime.Now,TemperatureFahrenheit=65},
            new City(){CityUniqueCode="KOL",CityName="Kolkata",DateAndTime=DateTime.Now,TemperatureFahrenheit=80},
            new City(){CityUniqueCode="CHN",CityName="Chennai",DateAndTime=DateTime.Now,TemperatureFahrenheit=70}
        };
            return ViewComponent("Table",Cities);
        }
    }
}
