using Microsoft.AspNetCore.Mvc;

namespace IAction_Result.Controllers
{
    public class BookControl : Controller
    {
        bool status = false;
        [Route("/book")]
        public IActionResult Index()
        {
            if(int.TryParse(Request.Query["id"],out int id) && bool.TryParse(Request.Query["LoggedIn"],out bool loggedIn))
            {

                    if (loggedIn == false)
                    {
                        return RedirectToAction("loginError","Home");
                    }
                    else
                    {
                        if (!status)
                        {
                            Response.StatusCode = 200;
                            status = true;
                        }

                        return Content($"\nBook id is {id}");
                    return StatusCode(200);
                    }
                
            }
            else
            {
                //if (!status)
                //{
                //    Response.StatusCode = 404;
                //    status = true;
                //}
                return RedirectToAction("InvalidInputError","Home");

            }
        }
    }
}
