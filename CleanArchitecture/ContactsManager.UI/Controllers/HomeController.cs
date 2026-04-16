using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [Route("[Action]")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;
            ViewBag.ErrorMessage = exception.Message;   
            return View();
        }
    }
}
