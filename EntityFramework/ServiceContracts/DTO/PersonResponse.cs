using Entities;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(5)]
        public string? CountryID { get; set; }

        [Range(100000, 999999)]
        public int? Pin { get; set; }

        public int GenderKey { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not PersonResponse other)
                return false;

            // Compare all relevant properties
            return PersonID == other.PersonID
                && string.Equals(PersonName, other.PersonName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase)
                && DateOfBirth == other.DateOfBirth
                && string.Equals(Gender, other.Gender, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Address, other.Address, StringComparison.OrdinalIgnoreCase)
                && string.Equals(CountryID, other.CountryID, StringComparison.OrdinalIgnoreCase)
                && Pin == other.Pin;
        }

        public override int GetHashCode()
        {
            // Combine hash codes of properties
            return HashCode.Combine(PersonID, PersonName, Email, DateOfBirth, Gender, Address, CountryID, Pin);
        }

    }

    public static class PersonExtension
     { 
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse() { PersonID = person.PersonID, PersonName = person.PersonName, Address = person.Address, CountryID = person.CountryID, DateOfBirth = person.DateOfBirth, Email = person.Email, Gender = person.Gender, Pin = person.Pin };
        }
     }
}
