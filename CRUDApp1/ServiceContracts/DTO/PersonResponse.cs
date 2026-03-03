using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using ServiceContracts.Enums;

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

        //When we Fetch the data of a person for updating, we get it in the form of PersonResponse, but to update the data of a person, we need to send it in the form of PersonUpdateRequest. So, we need to convert the PersonResponse object to PersonUpdateRequest object. For that, we can create a method in the PersonResponse class which will convert the PersonResponse object to PersonUpdateRequest object and return it.
        public PersonUpdateRequest ToPersonUpdateResponse()
        {
            return new PersonUpdateRequest() { PersonID = PersonID, PersonName = PersonName, Address = Address, DateOfBirth = DateOfBirth, Email = Email, CountryID = CountryID, Gender = (GenderValues)Enum.Parse(typeof(GenderValues), Gender) };
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
