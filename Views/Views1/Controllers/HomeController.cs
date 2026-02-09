using Microsoft.AspNetCore.Mvc;

namespace Views1.Controllers
{
    public class HomeController : Controller
    {
        [Route("/home/")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
