using ServicesContracts;
using ServicesContracts.DTO;
using Entities;

namespace Services
{
    public class PersonServices : IPersonServices
    {
        private readonly List<Person> _persons;

        public PersonServices()
        {
            _persons = new List<Person>() 
            {

            new Person
            {
                PersonID = Guid.NewGuid(),
                PersonName = "Alice Johnson",
                Email = "alice.johnson@example.com",
                DateOfBirth = new DateTime(1990, 5, 12),
                Gender = "Female",
                Address = "123 Maple Street, New York",
                CountryID = "US"
            },
            new Person
            {
                PersonID = Guid.NewGuid(),
                PersonName = "Bob Smith",
                Email = "bob.smith@example.com",
                DateOfBirth = new DateTime(1985, 11, 23),
                Gender = "Male",
                Address = "456 Oak Avenue, Chicago",
                CountryID = "US"
            },
            new Person
            {
                PersonID = Guid.NewGuid(),
                PersonName = "Catherine Lee",
                Email = "catherine.lee@example.com",
                DateOfBirth = new DateTime(1992, 2, 8),
                Gender = "Female",
                Address = "789 Pine Road, San Francisco",
                CountryID = "US"
            },
            new Person
            {
                PersonID = Guid.NewGuid(),
                PersonName = "David Kumar",
                Email = "david.kumar@example.com",
                DateOfBirth = new DateTime(1988, 7, 19),
                Gender = "Male",
                Address = "12 MG Road, Bengaluru",
                CountryID = "IN"
            },
            new Person
            {
                PersonID = Guid.NewGuid(),
                PersonName = "Emma Brown",
                Email = "emma.brown@example.com",
                DateOfBirth = new DateTime(1995, 9, 30),
                Gender = "Female",
                Address = "34 Queen Street, London",
                CountryID = "UK"
            }
            };
        }
        //method to add person
        public PersonResponse AddPerson(PersonAddRequests? personAddRequest)
        {
            //if the entire request is null
            if (personAddRequest == null) throw new ArgumentNullException("Add request is null. Adding Failed");

            //if any or many attribte of the request is null
            foreach(var property in typeof(PersonAddRequests).GetProperties())
            {
                throw new ArgumentException(nameof(property)+" is null. Adding Failed!");
            }

            //Actual adding logic
            Person personToAdd = personAddRequest.ConvertToPerson();
            //assigning person id
            personToAdd.PersonID = Guid.NewGuid();

            //Checking current count of person
            int personCount = _persons.Count();

            //adding to the person list
            _persons.Add(personToAdd);

            //checking if addition is successfull that is count increased
            if (_persons.Count() > personCount)//successfull addition
            {
                return personToAdd.ToPersonResponse();
            }
            else//addition failed
            {
                return null;
            } 
        }

        //return all person
        public List<PersonResponse> GetAllPersonResponseList() 
        { 
            List<PersonResponse> personResponses = new List<PersonResponse>();

            foreach(Person person in _persons)
            {
                personResponses.Add(person.ToPersonResponse());
            }

            return personResponses;
        }

        //return person on the basis of filter
        public List<PersonResponse> GetFilteredPersons(string? ByProperty, string? PropertyValue)
        {
            //if either of the filter parameter is null then return all person
            if (string.IsNullOrEmpty(ByProperty) || string.IsNullOrEmpty(PropertyValue))return GetAllPersonResponseList();

            List<string> PersonProperties = new List<string>();

            foreach (var property in typeof(Person).GetProperties())
            {
                PersonProperties.Add(property.Name);
            }

            if (!PersonProperties.Contains(ByProperty))
            {
                throw new ArgumentException("Property Name is invalid");
            }

            switch (ByProperty.ToLower())
            {
                case "personname":
                    return GetAllPersonResponseList().FindAll(p => p.PersonName != null && p.PersonName.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "email":
                    return GetAllPersonResponseList().FindAll(p => p.Email != null && p.Email.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "address":
                    return GetAllPersonResponseList().FindAll(p => p.Address != null && p.Address.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "personid":
                    return GetAllPersonResponseList().FindAll(p => p.PersonID != null && p.PersonID.ToString().Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "countryid":
                    return GetAllPersonResponseList().FindAll(p => p.CountryID != null && p.CountryID.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
                case "dateofbirth":
                    return GetAllPersonResponseList().FindAll(p => p.DateOfBirth != null && p.DateOfBirth.ToString().Contains(PropertyValue));
                case "gender":
                    return GetAllPersonResponseList().FindAll(p => p.Gender != null && p.Gender.ToString().ToLower().Equals(PropertyValue.ToLower()));

                default: return GetAllPersonResponseList();
            }
        }
    }
}
