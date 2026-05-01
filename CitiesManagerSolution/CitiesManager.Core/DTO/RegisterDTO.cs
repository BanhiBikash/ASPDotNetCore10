using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Person Name can't be blank")]
        public string? PeronName { get; set; }

        [Required(ErrorMessage ="Email address is required")]
        [EmailAddress(ErrorMessage ="Invalid email")]
        [Remote(action:"IsEmailAlreadyTaken", controller: "Account", ErrorMessage="Email address already in use! Change email or login.")]
        public string? Email {get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
