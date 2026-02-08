using Microsoft.AspNetCore.Mvc;
using CustomModelBinder.Models;
using CustomModelBinder.CustomModelBinders;

namespace CustomModelBinder.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index([ModelBinder(BinderType = typeof(NameBinder))] Person person)
        {
            return Content($"Name:{person.PersonName.Trim()} Address:{person.FullAddress.Trim()}","text/html");
        }
    }
}
