using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace WeatherAppServices.Controllers
{


    public class HomeController : Controller
    {
        public readonly ICitiesService _cityService;

        public HomeController(ICitiesService cityService)
        {
            _cityService = cityService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _cityService.GetCities();
            return View(cities);
        }

        //public IActionResult Index([FromServices] ICitiesService _cityService)
        //{
        //    List<string> cities = _cityService.GetCities();
        //    return View(cities);
        //}
    }
}
