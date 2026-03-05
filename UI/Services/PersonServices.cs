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
    }
}
