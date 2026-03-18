using Microsoft.AspNetCore.Mvc;

namespace StocksAppEntityFrameWork.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
