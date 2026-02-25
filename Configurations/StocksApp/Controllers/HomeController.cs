using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
