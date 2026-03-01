using System;
using System.Collections.Generic;
using System.Text;
using ServiceContracts;
using ServiceContracts.DTO;
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
    }
}
