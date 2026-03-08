using Entities;
using ServicesContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServicesContracts.DTO
{
    public class PersonAddRequests
    {
        [Required(ErrorMessage = "PersonName is required.")]
        public string? PersonName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public GenderValues? Gender { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required(ErrorMessage = "CountryID is required")]
        public string? CountryID { get; set; }

        public Person ConvertToPerson()
        {
            return new Person
            {
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                Address = this.Address,
                CountryID = this.CountryID
            };
        }
    }
}
