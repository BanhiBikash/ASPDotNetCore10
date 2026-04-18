using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactsManager.UI.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using ContactsManager.Core.ServiceContracts.Enums;

namespace ContactsManager.UI.Controllers
{
    //[AllowAnonymous]
    [Route("[Controller]")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Route("[Action]")]
        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("[Action]")]
        [HttpPost]
        [Authorize("NotAuthenticated")]
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
                //check roles
                if(registerDTO.UserType == UserType.Admin)
                {
                    //if admin role is not created then create first
                    if((await _roleManager.FindByNameAsync(UserType.Admin.ToString())) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = Convert.ToString(UserType.Admin) });
                    }

                    //add the user to admin role
                    await _userManager.AddToRoleAsync(user,UserType.Admin.ToString());
                }
                else
                { 
                    await _userManager.AddToRoleAsync(user, UserType.User.ToString());
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
        }

        [Route("[Action]")]
        [HttpGet]
        [TypeFilter(typeof(LogOutActionFilter))]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult?> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Persons");
        }

        [Route("[Action]")]
        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("[Action]")]
        [HttpPost]
        [TypeFilter(typeof(LoginActionFilter))]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                //for admin redirect
                ApplicationUser applicationUser = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (applicationUser != null) 
                { 
                    if(await _userManager.IsInRoleAsync(applicationUser, "Admin"))
                    {
                        return RedirectToAction("Index","Admin", new {area="Admin"});
                    }
                }

                //if there is a return url and it is local, redirect to it. Otherwise, redirect to the home page
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToAction(nameof(PersonsController.Index),"Persons");
            }
            else
            {
                ViewBag.Error = new List<string>() { "Login Failed! Check Email and Password" };
                return View(loginDTO);
            }
        }

        public async Task<IActionResult> IsEmailAlreadyTaken(string? email)
        {
            ApplicationUser? User = await _userManager.FindByEmailAsync(email);

            if(User == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
