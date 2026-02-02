using Microsoft.AspNetCore.Mvc;
using Content_Result_Json_Result.Models;

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

        //this returns a content result
        [Route("/person/")]
        public JsonResult PersonData()
        {
            Person man = new Person();
            man.id = 1;
            man.name = "Banhi";
            man.age = 25;
            return Json(man);
        }
    }
}
