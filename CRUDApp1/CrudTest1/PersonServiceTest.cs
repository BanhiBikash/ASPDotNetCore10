using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrudTest1
{
    public class PersonServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly List<PersonResponse>? _personsExpected;

        public PersonServiceTest()
        {
            _personsService = new PersonsService();
            _personsExpected = new List<PersonResponse>();
        }

        #region AddPerson
        /// <summary>
        /// Verifies that the AddPerson method throws an ArgumentNullException when a null request is provided.
        /// </summary>
        /// <remarks>This test ensures that the AddPerson method enforces its precondition of requiring a
        /// non-null PersonAddRequest parameter. Passing null should result in an ArgumentNullException, indicating
        /// correct input validation.</remarks>
        [Fact]
        public void AddPerson_NullRequest()
        {
            //Arrange
            PersonAddRequest? personaddRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.AddPerson(personaddRequest);
            });
        }

        /// <summary>
        /// Verifies that the AddPerson method throws an ArgumentException when provided with a null or invalid request
        /// parameter.
        /// </summary>
        /// <remarks>This unit test ensures that the AddPerson method enforces input validation by
        /// throwing the expected exception when the request parameter is not valid. Use this test to confirm that
        /// argument validation is correctly implemented in the service.</remarks>
        [Fact]
        public void AddPerson_NullRequestParaeter()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "null"};

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        /// <summary>
        /// Verifies that adding a valid person returns a response with a non-empty person identifier.
        /// </summary>
        /// <remarks>This unit test ensures that the AddPerson method of the persons service correctly
        /// generates a unique identifier when provided with valid person details. The test passes if the returned
        /// PersonResponse contains a non-empty PersonID.</remarks>
        [Fact]
        public void AddPerson_GetValidPersonResponse()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest() 
            { PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" };

            //Act
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            //Assert
            Assert.True(personResponse.PersonID != Guid.Empty);
        }
        #endregion
        
        #region GetPersonList

        [Fact]
        public void GetPersonList_ReturnsEmptyList()
        {
            //Arrange
            //No need to arrange anything as we are testing the initial state of the service.
            //Act
            List<PersonResponse> personList = _personsService.GetPersonList();
            //Assert
            Assert.Empty(personList);
        }

        [Fact]
        public void GetPersonList_ReturnAddedPersons()
        {
            //Arrange
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest>()
            { 
                new PersonAddRequest() {PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" },
                new PersonAddRequest()  {PersonName = "John Smith", Email = "john.smith@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, Camada", CountryID = "CND" }
            };

            //Act
            List<PersonResponse> personList = new List<PersonResponse>();

            foreach(PersonAddRequest person in personAddRequest)
            {
                personList.Add(_personsService.AddPerson(person));
            }

            //Assert
            foreach(var person in personList)
            {
                Assert.Contains(person, _personsService.GetPersonList());
            }
        }
        #endregion

        #region GetPerson

        [Fact]
        public void GetPersonByID_Null()
        {
            //Arrange
            Guid? personID = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.GetPersonByID((personID));
            });
        }

        [Fact]
        public void GetPersonByID_EmptyGuid()
        {
            //Arrange
            Guid empty = Guid.Empty;
            Guid personID = empty;
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.GetPersonByID(personID);
            });
        }

        [Fact]
        public void GetPersonByID_ProperPersonResponse()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            { PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" };

            //Act
            PersonResponse? personResponse = _personsService.AddPerson(personAddRequest);
            PersonResponse? responseFromGet = _personsService.GetPersonByID(personResponse.PersonID);

            //Assert
            Assert.Equal(personResponse,responseFromGet);
        }

        #endregion

        #region GetFilteredPerson

        /// <summary>
        /// Verifies that the GetFilteredPersons method returns an empty list when no filter criteria are provided.
        /// </summary>
        /// <remarks>This test ensures that the filtering logic in the persons service behaves as expected
        /// when called with empty filter parameters. It is intended to validate that all persons are returned when no
        /// filters are applied.</remarks>
        [Fact]
        public void GetFilteredPerson_ReturnsEmptyList()
        {
            //Arrange
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest>()
            {
                new PersonAddRequest() {PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" },
                new PersonAddRequest()  {PersonName = "John Smith", Email = "john.smith@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, Camada", CountryID = "CND" }
            };

            //Act
            foreach (PersonAddRequest person in personAddRequest)
            {
                _personsExpected.Add(_personsService.AddPerson(person));
            }

            List<PersonResponse> filteredPersons = _personsService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

            //Assert_personsService.GetFilteredPersons(null, null)
            foreach (PersonResponse person in _personsExpected)
            {
                Assert.Contains(person, filteredPersons);
            }
        }

        /// <summary>
        /// Verifies that the GetFilteredPersons method returns only the persons matching the specified search criteria
        /// for the person name.
        /// </summary>
        /// <remarks>This test adds multiple persons to the service and asserts that filtering by person
        /// name returns the expected subset. It ensures that the filtering logic correctly identifies and returns
        /// persons whose names match the search term, and that no unexpected persons are included in the
        /// results.</remarks>
        [Fact]
        public void GetFilteredPerson_ReturnsSearchedPerson()
        {
            //Arrange
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest>()
            {
                new PersonAddRequest() {PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" },
                new PersonAddRequest()  {PersonName = "John Smith", Email = "john.smith@example1.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, Camada", CountryID = "CND" },
                new PersonAddRequest()  {PersonName = "John Smith", Email = "john.smith@example2.com", DateOfBirth = new DateTime(2000, 5, 15), Gender = GenderValues.Male, Address = "Delhi", CountryID = "IND" }
            };

            //Act
            foreach (PersonAddRequest person in personAddRequest)
            {
                _personsExpected.Add(_personsService.AddPerson(person));
            }

            List<PersonResponse> expectedList = _personsExpected.Where(person=>person.PersonName.Contains("John Smith",StringComparison.OrdinalIgnoreCase)).ToList();
            List<PersonResponse> filteredPersons = _personsService.GetFilteredPersons(nameof(Person.PersonName), "John Smith");

            //Assert_personsService.GetFilteredPersons checking that the filtered list contains only the expected person and that all expected persons are in the filtered list.
            foreach (PersonResponse person in expectedList)
            {
                Assert.Contains(person, filteredPersons);
            }
            foreach (PersonResponse person in filteredPersons)
            {
                Assert.Contains(person, expectedList);
            }
        }
        #endregion
    }
}
