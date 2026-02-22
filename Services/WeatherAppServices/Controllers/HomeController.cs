using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace WeatherAppServices.Controllers
{


    public class HomeController : Controller
    {
        public readonly ICitiesService _cityService;
        public readonly ISpecificCity _specificCity;

        public HomeController(ICitiesService cityService, ISpecificCity specificCity)
        {
            _cityService = cityService;
            _specificCity = specificCity;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<City> cities = _cityService.GetCities();
                            return View(cities);
        }

        [Route("/CityPage/{CityCode}")]
        public IActionResult SpecificCity(string CityCode)
        {
            City? CityData = _specificCity.GetCityDetails(CityCode);
            return View(CityData);
        }

        //public IActionResult Index([FromServices] ICitiesService _cityService)
        //{
        //    List<string> cities = _cityService.GetCities();
        //    return View(cities);
        //}
    }
}
