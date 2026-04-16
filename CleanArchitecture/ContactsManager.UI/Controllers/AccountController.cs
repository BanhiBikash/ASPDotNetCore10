using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactsManager.UI.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;

namespace ContactsManager.UI.Controllers
{
    [AllowAnonymous]
    [Route("[Controller]")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            //add user to database
            IdentityResult result = await _userManager.CreateAsync(user,registerDTO.Password);

            if(!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerDTO);
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
        }

        [Route("[Action]")]
        [HttpGet]
        [TypeFilter(typeof(LogOutActionFilter))]
        public async Task<IActionResult?> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Persons");
        }

        [Route("[Action]")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("[Action]")]
        [HttpPost]
        [TypeFilter(typeof(LoginActionFilter))]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(PersonsController.Index),"Persons");
            }
            else
            {
                ViewBag.Error = new List<string>() { "Login Failed! Check Email and Password" };
                return View(loginDTO);
            }
        }
    }
}
