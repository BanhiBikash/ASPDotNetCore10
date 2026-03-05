using Entities;
using ServicesContracts.Enums;

namespace ServicesContracts.DTO
{
    public class PersonAddRequests
    {
        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderValues? Gender { get; set; }

        public string? Address { get; set; }

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
