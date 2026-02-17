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

        [Route("/Cities")]
        public IActionResult CitiesPage()
        {
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
