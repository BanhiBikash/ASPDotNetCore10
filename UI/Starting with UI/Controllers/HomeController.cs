using Microsoft.AspNetCore.Mvc;

namespace Starting_with_UI.Controllers
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
