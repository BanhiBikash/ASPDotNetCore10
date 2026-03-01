using System;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderValues? Gender { get; set; }

        public string? Address { get; set; }

        public string? CountryID { get; set; }

        public Person ToPerson()
        {
            return new Person() { PersonName = PersonName, Address = Address, DateOfBirth = DateOfBirth, Email = Email, Gender = Gender.ToString(), CountryID = CountryID };
        }
    }
}
