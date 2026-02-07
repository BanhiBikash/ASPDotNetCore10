using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace Form_URLEncoded_and_Form_Data.Custom_Validations
{
    public class MinimumDateofBirth:ValidationAttribute
    {
        public int minYear {get; set;}

        int currentYear = DateTime.Now.Year;

        //non-parametrized constructor
        public MinimumDateofBirth() { }

        //parametrized constructor
        public MinimumDateofBirth(int minYear)
        {
            this.minYear= minYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;

            string ErrorMessageDefault = $"Candidate shouldn't be older than {currentYear-date.Year}s old.";

            if (date != null)
            {

                if (date.Year>currentYear)
                {
                    return new ValidationResult("Enter a valid date of birth");
                }
                else if (date.Year < minYear)
                {
                    return new ValidationResult(ErrorMessage??ErrorMessageDefault);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
