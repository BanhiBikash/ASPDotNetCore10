using Form_URLEncoded_and_Form_Data.Custom_Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Form_URLEncoded_and_Form_Data.Models
{
    public class Students
    {
        //this takes value in accordnace to precedence
        [Display(Name = "Student Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "{0} shall be between {2} and {1} characters long.")]
        public string? Name { get; set; }

        //this takes value in accordnace to precedence
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(1, 100, ErrorMessage = "{0} shall be between {1} and {2}.")]
        public int? Id { get; set; }

        //this takes value in accordnace to precedence
        [Display(Name = "Student City")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "{0} shall be between {2} and {1} characters long.")]
        [CityChoices]
        public string? City { get; set; }


        //this takes value in accoradance to precedence
        [Required(ErrorMessage = "Date of Birth can't be empty.")]
        [MinimumDateofBirth(2008, ErrorMessage = "The candidate can't be older than 16 years old.")]
        public DateTime dob { get; set; }
    }
}
