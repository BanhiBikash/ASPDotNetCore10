using Form_URLEncoded_and_Form_Data.Custom_Validations;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
namespace Form_URLEncoded_and_Form_Data.Models
{
    public class Students:IValidatableObject
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
        //[Required(ErrorMessage = "Date of Birth can't be empty.")]
        [MinimumDateofBirth(2008, ErrorMessage = "The candidate can't be older than 16 years old.")]
        public DateTime? dob { get; set; }

        //takes value according to prevelance 
        public int? age { get; set; }

        [Required(ErrorMessage = "enter the calss you want the candidate to apply for.")]
        [AppliedClassAttribute(ErrorMessage = "The class applied should be between 1 and 12.")]
        public int classApplied{ get; set; }

        [Required(ErrorMessage = "Enter the last class studied by the candidate.")]
        [LastClassValidator("classApplied", ErrorMessage = "The last class studied should be less than the class applied for.")]
        public int LastClassStudies { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(dob.HasValue == false && age.HasValue == false)
            {
               yield return new ValidationResult("Either DOB or age must be supplied.", new[] {nameof(dob), nameof(age)});
            }
        }
    }
}
