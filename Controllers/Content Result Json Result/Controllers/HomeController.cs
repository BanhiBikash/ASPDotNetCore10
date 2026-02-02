using Microsoft.AspNetCore.Mvc;

namespace Content_Result_Json_Result.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public ContentResult Index()
        {
            return Content("\nReturn from content type","text/plain");
        }

        [Route("/home/")]
        public ContentResult Home()
        {
            return Content("<h1>This is Home page.</h1>", "text/html");
        }
    }
}
