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
    }
}
