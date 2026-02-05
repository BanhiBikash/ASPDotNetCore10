using Microsoft.AspNetCore.Mvc;
using Form_URLEncoded_and_Form_Data.Models;

namespace Form_URLEncoded_and_Form_Data.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Home Controller of form data!", "text/html");
        }

        //receives student type parameters
        [Route("/students/{Name?}/{Id:int?}/{City?}")]
        public IActionResult Data(Students students)
        {
            return Content($"\n-----------\nStudent Name:{students.Name}  Id:{students.Id}  City:{students.City}\n---------\n ", "text/html");
        }
    }
}
