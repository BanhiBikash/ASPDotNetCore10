using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Form_URLEncoded_and_Form_Data.Custom_Validations
{
    public class LastClassValidator:ValidationAttribute
    {
        public string OldPropertyName { get; set; }

        public LastClassValidator(string OldPropertyName) 
        {
            this.OldPropertyName = OldPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int newClassApplied = (int)value;
                PropertyInfo? OldPropertyInstance= validationContext.ObjectType.GetProperty(this.OldPropertyName);
                int LastClass = (int)OldPropertyInstance.GetValue(validationContext.ObjectInstance);

                if (LastClass != null)
                {
                    //applied for class skipping last studied class
                    if(LastClass<=newClassApplied)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                    else if (newClassApplied>LastClass+1)
                    {
                        return new ValidationResult("The candidate can't skip one class.");
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
            else
            {
                return null;
            }
        }
    }
}
