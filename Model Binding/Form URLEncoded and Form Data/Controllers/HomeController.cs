using Microsoft.AspNetCore.Mvc;
using Form_URLEncoded_and_Form_Data.Models;
using System.Threading.Tasks.Dataflow;

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
            //ModelSate stores all the values received during model binding and model validation

            if (!ModelState.IsValid)
            {
                String ErrorMessage = String.Join("\n",ModelState.Values.SelectMany(Value=>Value.Errors).Select(err=>err.ErrorMessage));
                return BadRequest(ErrorMessage);
            }
            else
            {
                return Content($"\n-----------\nStudent Name:{students.Name},  Id:{students.Id},  City:{students.City},  Date of Birth:{students.dob.ToString("dd/mm/yyyy")}, Class Applied: {students.classApplied}, Last Class Studied:{students.LastClassStudies}\n---------\n ", "text/html");
            }
        }
    }
}
