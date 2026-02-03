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
                        if (!status)
                        {
                            Response.StatusCode = 200;
                            status = true;
                        }

                        return Content("\nPlease login");
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
                return Content("\nEither invalid or empty data delivered.");
                return StatusCode(400);

            }
        }
    }
}
