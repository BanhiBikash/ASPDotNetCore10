using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        public Guid? PersonID { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderValues? Gender { get; set; }

        public string? Address { get; set; }

        public string? CountryID { get; set; }

        public Person ToPerson()
        {
            return new Person() {PersonID = PersonID, PersonName = PersonName, Address = Address, DateOfBirth = DateOfBirth, Email = Email, Gender = Gender.ToString(), CountryID = CountryID };
        }
    }
}
