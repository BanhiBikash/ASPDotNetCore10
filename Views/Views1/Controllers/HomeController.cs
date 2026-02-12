using Microsoft.AspNetCore.Mvc;
using Views1.Models;

namespace Views1.Controllers
{
    public class HomeController : Controller
    {
        [Route("/home/")]
        [Route("/")]
        public IActionResult Index()
        {
            List<Student> students = new List<Student>()
            {
                new Student(){name="John", roll=1, std=10},
                new Student(){name="Alice", roll=2, std=9},
                new Student(){name="Bob", roll=3, std=8}
            };

            ViewData["students"] = students;
            return View();
        }
    }
}
