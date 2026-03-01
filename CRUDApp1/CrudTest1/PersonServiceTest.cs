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

        public PersonServiceTest()
        {
            _personsService = new PersonsService();
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
    }
}
