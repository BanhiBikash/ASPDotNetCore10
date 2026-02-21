using Microsoft.AspNetCore.Mvc;

namespace WeatherAppServices.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
