using Asp.Versioning;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Entities.IdentityUser;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.ServiceContracts.Enums;
using CitiesManager.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CitiesManager.web.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJWTService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJWTService jWTService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jWTService;
        }

        /// <summary>
        /// Registers a new user account with the specified registration details.
        /// </summary>
        /// <remarks>If the specified email address is already registered, the method returns a problem
        /// response. The method also assigns the user to the specified role, creating the role if it does not exist.
        /// The user is automatically signed in upon successful registration. This endpoint expects a valid RegisterDTO
        /// payload and should be called via HTTP POST.</remarks>
        /// <param name="registerDTO">An object containing the user's registration information, including email, password, display name, desired
        /// role, and sign-in preferences. Cannot be null. All required fields must be valid.</param>
        /// <returns>An ActionResult containing the created ApplicationUser if registration succeeds; otherwise, a problem
        /// response describing the error (such as validation failures or duplicate email).</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUser>> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                string? errors = string.Join(",", ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage));
                return Problem(errors);
            }

            if ((await _userManager.FindByEmailAsync(registerDTO.Email)) != null)
            {
                return Problem("Email already registered");
            }
            else
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    PersonName = registerDTO.PersonName,
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                };

                IdentityResult createResult = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

                if (createResult.Succeeded)
                {
                    //sign-in user
                    await _signInManager.SignInAsync(applicationUser, isPersistent: registerDTO.stayLoggedIn);

                    if (registerDTO.UserRole == Role.User.ToString())
                    {
                        if (await _roleManager.FindByNameAsync(Convert.ToString(Role.User)) is null)
                        {
                            try
                            {
                                await _roleManager.CreateAsync(new ApplicationRole() { Name = Role.User.ToString() });
                            }
                            catch (Exception e)
                            {
                                Problem($"Failed to create {Role.User.ToString()} role.");
                            }
                        }

                        await _userManager.AddToRoleAsync(applicationUser, Role.User.ToString());

                        AuthResponseDTO authResponse = _jwtService.CreateJWTToken(applicationUser);

                        return Ok(authResponse);
                    }
                    else if (registerDTO.UserRole == Role.Admin.ToString())
                    {
                        if (await _roleManager.FindByNameAsync(Convert.ToString(Role.Admin)) is null)
                        {
                            try
                            {
                                await _roleManager.CreateAsync(new ApplicationRole() { Name = Role.Admin.ToString() });
                            }
                            catch (Exception e)
                            {
                                return Problem($"Failed to create {Role.Admin.ToString()} role.");
                            }
                        }

                        await _userManager.AddToRoleAsync(applicationUser, Role.Admin.ToString());
                        return Ok(applicationUser);
                    }
                    else
                    {
                        return Problem("Either role not assigned or invalid role");
                    }
                }
                else
                {
                    string? errors = String.Join(",", createResult.Errors.Select(err => err.Description));
                    return Problem(errors);
                }
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<ActionResult<ApplicationUser>> Login(LoginDTO loginDTO)

        {
            if (loginDTO is null)
            {
                return Problem("Invalid login data");
            }
            else
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user is null)
                {
                    return Problem("Invalid email or password");
                }
                else
                {
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, isPersistent: loginDTO.stayLoggedIn, lockoutOnFailure: false);
                    
                    if (signInResult.Succeeded)
                    {
                        AuthResponseDTO authResponse = _jwtService.CreateJWTToken(user);

                        return Ok(authResponse);
                    }
                    else
                    {
                        return Problem("Wrong email or password.");
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the specified email address is already associated with an existing user account.
        /// </summary>
        /// <param name="email">The email address to check for availability. Can be null.</param>
        /// <returns>An <see cref="OkObjectResult"/> containing <see langword="true"/> if the email is already taken; otherwise,
        /// <see langword="false"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyTaken(string? email)
        {
            if (_userManager.FindByEmailAsync(email) is null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }
    }
}
