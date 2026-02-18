using Microsoft.AspNetCore.Mvc;

namespace PartialView1.Controllers
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

        [Route("/CityPage")]
        public IActionResult CityPage()
        {
            return View();
        }

        [Route("/Programming_Languages")]
        public IActionResult Programming_Languages()
        {
            return View();
        }
    }
}
