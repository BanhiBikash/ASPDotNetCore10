using Entities;

namespace ServicesContracts.DTO
{
    public class PersonResponse
    {
        public Guid? PersonID { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? CountryID { get; set; }
    }

    public static class PersonExtension
     { 
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse() { PersonID = person.PersonID, PersonName = person.PersonName, Address = person.Address, CountryID = person.CountryID, DateOfBirth = person.DateOfBirth, Email = person.Email, Gender = person.Gender };
        }
     }
}
