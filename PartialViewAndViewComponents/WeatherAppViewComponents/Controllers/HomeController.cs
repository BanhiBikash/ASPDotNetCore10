using Microsoft.AspNetCore.Mvc;
using WeatherAppViewComponents.Models;

namespace WeatherAppViewComponents.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/LoadSpecificCity/{cityCode:required}")]
        public IActionResult CitySpecific(string cityCode)
        {
            return ViewComponent("SpecificCity",cityCode);
        }
    }
}
