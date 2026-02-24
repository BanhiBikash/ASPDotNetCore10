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
            ViewBag.ChildKey1 = _configuration.GetValue<string>("MasterKey:ChildKey1");

            //get section property
            IConfigurationSection MasterSection = _configuration.GetSection("MasterKey");
            ViewBag.ChildKey2 = MasterSection["ChildKey2"];

            return View();
        }
    }
}
