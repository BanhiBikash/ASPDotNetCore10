using Microsoft.AspNetCore.Mvc;

namespace ConfigAppSettings.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.ConfigData = _configuration.GetValue<string>("MyKey");
            return View();
        }
    }
}
