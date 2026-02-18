using Microsoft.AspNetCore.Mvc;
using PartialView1.Models;

namespace PartialView1.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/CityPage")]
        public IActionResult CityPage()
        {
            return View();
        }

        [Route("/Programming_Languages")]
        public IActionResult Programming_Languages()
        {
            return View();
        }

        [Route("/GetCars/")]
        public IActionResult CarsList()
        {
            ListModels CarList = new ListModels()
            {
                ListTitle="Car List",
                Items = new List<string>
                {
                    "BMW",
                    "Mercedes",
                    "Tata",
                    "Mahindra"
                }
            };

            return PartialView("_PartialContent",CarList);
        }

        [Route("Cars/")]
        public IActionResult Cars()
        {
            return View();
        }
    }
}
