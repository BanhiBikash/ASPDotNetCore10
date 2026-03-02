using System;
using System.Collections.Generic;
using System.Text;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Entities;
using System.Reflection;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;

        public PersonsService()
        {
            _persons = new List<Person>();
        }

        public PersonResponse AddPerson(PersonAddRequest request)
        {
            //if the request is null
            if(request == null) throw new ArgumentNullException(nameof(request));

            //if any parameter in the request is null, throw an exception
            foreach (var property in typeof(PersonAddRequest).GetProperties())
            {
                if (property.GetValue(request) == null)
                {
                    throw new ArgumentException(property.Name);
                }
            }

            //Validation
            ValidationHelpers.Validate(request);

            //Logic
            Person person = request.ToPerson();
            person.PersonID = Guid.NewGuid();

            _persons.Add(person);

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetPersonList()
        {
            List<PersonResponse> responseList = new List<PersonResponse>();

            foreach (var person in _persons)
            {
                responseList.Add(person.ToPersonResponse());
            }

            return responseList;
        }

        public PersonResponse? GetPersonByID(Guid? personID)
        {
            if(personID == null)
            {
                throw new ArgumentNullException("Person ID can't be null value.",nameof(personID));  
            }

            if(personID == Guid.Empty)
            {
                throw new ArgumentException("Person ID cannot be empty.", nameof(personID));
            }

            Person? person = _persons.Find(p => p.PersonID == personID);

            if (person == null) return null;
            
            return person.ToPersonResponse();
        }

        /// <summary>
        /// Retrieves a list of persons filtered by a specified property and value.
        /// </summary>
        /// <remarks>If PropertyValue is null, the method may return all persons or none, depending on the
        /// property and its handling of null values. Filtering is performed using a case-insensitive comparison for
        /// string properties. For properties that are not strings, the method attempts to convert and compare values as
        /// strings.</remarks>
        /// <param name="ByProperty">The name of the property to filter by. Must match a property of the Person class (e.g., "PersonName",
        /// "Email", "Address", "PersonID", "CountryID", or "DateOfBirth"). The comparison is case-insensitive.</param>
        /// <param name="PropertyValue">The value to match against the specified property. The filter is applied using a case-insensitive contains
        /// or equals comparison, depending on the property.</param>
        /// <returns>A list of PersonResponse objects that match the specified filter criteria. Returns all persons if the
        /// property name does not match a supported filter.</returns>
        /// <exception cref="ArgumentException">Thrown if ByProperty does not correspond to a valid property name of the Person class.</exception>
        public List<PersonResponse> GetFilteredPersons(string? ByProperty, string? PropertyValue)
        {
            List<string> PersonProperties = new List<string>();

            foreach(var property in typeof(Person).GetProperties())
            {
                PersonProperties.Add(property.Name.ToLower());
            }

            if (!PersonProperties.Contains(ByProperty.ToLower()))
            {
                throw new ArgumentException("Property Name is invalid");
            }

            switch (ByProperty.ToLower())
            {
                case "personname":
                    return GetPersonList().FindAll(p => p.PersonName != null && p.PersonName.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "email":
                    return GetPersonList().FindAll(p => p.Email != null && p.Email.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "address":
                    return GetPersonList().FindAll(p => p.Address != null && p.Address.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "personid":
                    return GetPersonList().FindAll(p => p.PersonID != null && p.PersonID.ToString().Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "countryid":
                    return GetPersonList().FindAll(p => p.CountryID != null && p.CountryID.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "dateofbirth":
                    return GetPersonList().FindAll(p => p.DateOfBirth != null && p.DateOfBirth.ToString().Contains(PropertyValue));

                default: return GetPersonList();
            }
        }

        public List<PersonResponse> GetSortedPersons(string? ByProperty,bool ascending = true)
        {
            if(string.IsNullOrEmpty(ByProperty))
            {
                throw new ArgumentNullException("Property Name cannot be null or empty.", nameof(ByProperty));
            }

            //Getting property info of the property name provided in the parameter
            var propInfo = typeof(PersonResponse).GetProperty(ByProperty);

                             //ascending order                                                    descending order
            return ascending?GetPersonList().OrderBy(p => propInfo.GetValue(p, null)).ToList() : GetPersonList().OrderByDescending(p => propInfo.GetValue(p, null)).ToList();
        }
    }
}
