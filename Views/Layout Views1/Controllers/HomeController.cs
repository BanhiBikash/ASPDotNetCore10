using Microsoft.AspNetCore.Mvc;

namespace Layout_Views1.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Cities/{CityCode?}")]
        public IActionResult CitiesPage(string? cityCode)
        {
            ViewBag.CityCode = Request.RouteValues["CityCode"];
            return View();
        }

        [Route("/About")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}
