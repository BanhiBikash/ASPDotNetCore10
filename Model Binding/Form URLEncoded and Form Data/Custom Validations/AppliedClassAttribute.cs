using System.ComponentModel.DataAnnotations;

namespace Form_URLEncoded_and_Form_Data.Custom_Validations
{
    public class AppliedClassAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int classApplied = (int)value;
                if(classApplied < 1 || classApplied > 12)
                {
                    return new ValidationResult(ErrorMessage);
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
