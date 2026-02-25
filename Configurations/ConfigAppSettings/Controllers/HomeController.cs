using Microsoft.AspNetCore.Mvc;
using ConfigAppSettings.Models;
using Microsoft.Extensions.Options;

namespace ConfigAppSettings.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly Keys _keys;

        public HomeController(IConfiguration configuration, IOptions<Keys> keys)
        {
            _configuration = configuration;
            _keys = keys.Value;
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

        [Route("/options")]
        public IActionResult Options()
        {
            Keys keys = new Keys();

            _configuration.GetSection("MasterKey").Bind(keys);

            ViewBag.Key1Options = keys.ChildKey1;
            ViewBag.Key2Options = keys.ChildKey2;

            return View();
        }

        [Route("OptionsAsService/")]
        public IActionResult OptionsAsService()
        {
            return View(new {_keys.ChildKey1, _keys.ChildKey2});
        }
    }
}
