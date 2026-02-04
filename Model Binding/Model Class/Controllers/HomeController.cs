using Microsoft.AspNetCore.Mvc;
using Model_Class.Models;

namespace Model_Class.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Home Controller!", "text/html");
        }

        //receives student type parameters
        [Route("/students/{Name?}/{Id:int?}/{City?}")]
        public IActionResult Data(Students students)
        {
            return Content($"\n-----------\nStudent Namr:{students.Name}  Id:{students.Id}  City:{students.City}\n---------\n ", "text/html");
        }
    }
}
