using Entities;
using ServicesContracts;
using ServicesContracts.DTO;
using System.Reflection;

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
                if(property.GetValue(personAddRequest) == null) throw new ArgumentException(nameof(property)+" is null. Adding Failed!");
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

        public bool DeletePerson(Guid? id)
        {
            if(id== null) throw new ArgumentNullException("Person ID is null. Deletion Failed");

            List<PersonResponse>? personToDelete = GetFilteredPersons("PersonID", id.ToString());

            if(personToDelete.Count == 0) throw new ArgumentException("No person found with the given ID. Deletion Failed");

            int numberRemoved = _persons.RemoveAll(person => person.PersonID == personToDelete[0].PersonID);

            if(numberRemoved > 0) //successfull deletion
            {
                return true;
            }
            else //deletion failed
            {
                return false;
            }
        }

        public bool EditPerson(PersonResponse? personResponse)
        {
            if (personResponse == null) throw new ArgumentNullException("Edit request is null. Editing Failed");

            foreach (var property in typeof(PersonResponse).GetProperties())
            {
                if (property.GetValue(personResponse) == null)
                    throw new ArgumentException(nameof(property) + "is null.");
            }

            List<PersonResponse>? personToEdit = GetFilteredPersons("PersonID", personResponse.PersonID.ToString());

            Person existingPerson = _persons.FirstOrDefault(person => person.PersonID == personToEdit[0].PersonID);

            existingPerson.PersonName = personResponse.PersonName;
            existingPerson.Email = personResponse.Email;
            existingPerson.DateOfBirth = personResponse.DateOfBirth;
            existingPerson.Gender = personResponse.Gender;
            existingPerson.CountryID = personResponse.CountryID;
            existingPerson.Address = personResponse.Address;

            PersonResponse updatedPersonResponse = existingPerson.ToPersonResponse();   
            //check if the changes took place
            if (personResponse.Equals(updatedPersonResponse))
            {
                return true;
            }
            else
            {
                return false;
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

        //return person on the basis of sorting
        public List<PersonResponse> GetSortedPersons(string? ByProperty, bool ascending = true)
        {
            if (string.IsNullOrEmpty(ByProperty))
            {
                throw new ArgumentNullException("Property Name cannot be null or empty.", nameof(ByProperty));
            }

            //Getting property info of the property name provided in the parameter
            var propInfo = typeof(PersonResponse).GetProperty(ByProperty,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null)
            {
                throw new ArgumentException($"Property '{ByProperty}' not found on PersonResponse.");
            }


            //ascending order                                                    descending order
            return ascending ? GetAllPersonResponseList().OrderBy(p => propInfo.GetValue(p, null)).ToList() : GetAllPersonResponseList().OrderByDescending(p => propInfo.GetValue(p, null)).ToList();
        }
    }
}
