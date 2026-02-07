using System.ComponentModel.DataAnnotations;

namespace Form_URLEncoded_and_Form_Data.Custom_Validations
{
    public class CityChoices:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? city = (string)value;
            city = city.Trim().ToLower();

            List<string> choices = new List<string>() { "pune", "mumbai", "delhi", "bangalore" };
            
            if(string.IsNullOrEmpty(city))
            {
                return null;
            }
            else if(choices.Contains(city))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Our schools are available in : {string.Join(", ", choices)} only.");
            }
        }
    }
}
