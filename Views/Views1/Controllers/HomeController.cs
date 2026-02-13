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
                new Student(){name="John", roll=1, std=10, gender="Male", age=17},
                new Student(){name="Alice", roll=2, std=9, gender="Female", age=16},
                new Student(){name="Bob", roll=3, std=8, gender="Male", age=18}
            };

            ViewData["students"] = students;
            return View();
        }

        [Route("/details/{studentName}")]
        public IActionResult Details(String? studentName)
        {
            if (studentName == null)
            {
                return BadRequest("Student Name not delivered.");      
            }
            else
            {
                Student? student = null;

                List<Student> students = new List<Student>()
            {
                new Student(){name="John", roll=1, std=10, gender="Male", age=17},
                new Student(){name="Alice", roll=2, std=9, gender="Female", age=16},
                new Student(){name="Bob", roll=3, std=8, gender="Male", age=18}
            };

                foreach (Student st in students)
                {
                    if (st.name.ToLower().Equals(studentName?.ToLower()))
                    {
                        student = st;
                        break;
                    }
                }

                if (student == null)
                {
                    return NotFound("this");
                }

                return View(student);

            }
        }
    }
}
