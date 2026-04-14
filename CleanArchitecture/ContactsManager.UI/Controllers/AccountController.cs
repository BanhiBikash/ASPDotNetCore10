using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactsManager.UI.Filters.ActionFilters;

namespace ContactsManager.UI.Controllers
{
    [Route("[Controller]")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("[Action]")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("[Action]")]
        [HttpPost]
        [TypeFilter(typeof(RegisterPostActionFilter))]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            return RedirectToAction(nameof(PersonsController.Index), "Persons" );
        }
    }
}
