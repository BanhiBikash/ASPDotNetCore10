using Microsoft.AspNetCore.Mvc;

namespace IAction_Result.Controllers
{
    public class HomeController : Controller
    {
        [Route("/redirect/LoginError")]
        public IActionResult LoginError()
        {
            return Content("\nThe user has not logged in.");
            return new StatusCodeResult(302);
        }

        [Route("/redirect/InvalidInput")]
        public IActionResult InvalidInputError()
        {
            return Content("\nThe user has entered invalid input.");
            return new StatusCodeResult(301);
        }
    }
}
