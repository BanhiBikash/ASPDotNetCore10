using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace ServiceContracts.DTO
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


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse personResponse = (PersonResponse)obj;
            if (this.PersonID == personResponse.PersonID && this.PersonName == personResponse.PersonName) return true;

            return false;
        }
    }

    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse() {PersonName = person.PersonName, Address = person.Address, DateOfBirth = person.DateOfBirth, Email = person.Email, CountryID = person.CountryID, Gender = person.Gender, PersonID = person.PersonID };
            
        }
    }
}
