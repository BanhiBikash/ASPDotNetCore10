using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[Controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [Route("[Action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
