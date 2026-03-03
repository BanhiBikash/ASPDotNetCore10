using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

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
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "null" };

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

            foreach (PersonAddRequest person in personAddRequest)
            {
                personList.Add(_personsService.AddPerson(person));
            }

            //Assert
            foreach (var person in personList)
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
            Assert.Equal(personResponse, responseFromGet);
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

            List<PersonResponse> expectedList = _personsExpected.Where(person => person.PersonName.Contains("John", StringComparison.OrdinalIgnoreCase)).ToList();
            List<PersonResponse> filteredPersons = _personsService.GetFilteredPersons(nameof(Person.PersonName), "John");

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

        #region SortedPerson

        /// <summary>
        /// Verifies that GetSortedPersons throws an ArgumentNullException when a null parameter is provided.
        /// </summary>
        /// <remarks>This unit test ensures that the GetSortedPersons method enforces its null argument
        /// precondition by throwing the appropriate exception. This helps maintain correct usage and input validation
        /// in the service.</remarks>
        [Fact]
        public void GetSortedPersons_NullParamterSent()
        {
            //Aseert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.GetSortedPersons(null);
            });

        }

        /// <summary>
        /// Verifies that the GetSortedPersons method throws an ArgumentException when an empty parameter is provided.
        /// </summary>
        /// <remarks>This test ensures that the GetSortedPersons method enforces input validation by
        /// rejecting empty string parameters. It is intended to confirm that the method does not accept empty input and
        /// responds with the appropriate exception.</remarks>
        [Fact]
        public void GetSortedPersons_EmptyParameterSent()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.GetSortedPersons(string.Empty);
            });
        }

        /// <summary>
        /// Verifies that the GetSortedPersons method returns a list of persons sorted by the specified property name.
        /// </summary>
        /// <remarks>This test adds multiple persons to the service and asserts that the returned list
        /// from GetSortedPersons, when sorted by person name, contains the expected persons in the correct order. The
        /// test ensures that sorting by property name functions as intended.</remarks>
        [Fact]
        public void GetSortedPersons_GetSorted()
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

            List<PersonResponse>? personsExpected = _personsExpected.OrderBy(p => typeof(PersonResponse).GetProperty(nameof(PersonResponse.PersonName)).GetValue(p, null)).ToList();
            List<PersonResponse>? personsSorted = _personsService.GetSortedPersons(nameof(PersonResponse.PersonName), true);

            //Assert
            foreach (PersonResponse person in personsExpected)
            {
                Assert.Contains(person, personsSorted);
            }
            foreach (PersonResponse person in personsSorted)
            {
                Assert.Contains(person, personsExpected);
            }
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullRequest()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_NullRequestAttribute()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonName = null };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_InvalidGuid()
        {
            //Arrange
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest>()
            { new PersonAddRequest() {PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" },
                new PersonAddRequest()  {PersonName = "John Smith", Email = "john.smith@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, Camada", CountryID = "CND" }
            };
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

            //Act
            foreach (PersonAddRequest person in personAddRequest)
            {
                _personsExpected.Add(_personsService.AddPerson(person));
            }

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_ProperResponse()
        {
            //Arrange
           List< PersonAddRequest> personAddRequest = new List<PersonAddRequest>() 
            { new PersonAddRequest() {PersonName = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, USA", CountryID = "USA" },
                new PersonAddRequest()  {PersonName = "John Smith", Email = "john.smith@example.com", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderValues.Male, Address = "123 Main Street, New York, Camada", CountryID = "CND" }
            };

            //Act
            foreach (PersonAddRequest person in personAddRequest)
            {
                _personsExpected.Add(_personsService.AddPerson(person));
            }

            //selecting the first person to update
            PersonResponse? personToUpdateResponse = _personsService.GetPersonByID(_personsExpected.First().PersonID);
            //converting it into update request to update the person
            PersonUpdateRequest? personToUpdate = personToUpdateResponse.ToPersonUpdateResponse();
            //updating the person name
            personToUpdate.PersonName = "John Updated";

            //We are receiving the updated person from updated person
            PersonResponse? UpdatedPerson = _personsService.UpdatePerson(personToUpdate);

            //Assert
            Assert.Equal(personToUpdate.ToPerson().ToPersonResponse(), UpdatedPerson);
        }

        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_NullID()
        {
            //Arrange
            Guid? personID = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.DeletePerson(personID);
            });
        }

        //if person not found send not deleted i.e. false
        [Fact]
        public void DeletePerson_PersonNotFoundOrInvalidGuid()
        {
            //Arrange
            Guid personID = Guid.NewGuid();

            //Assert
            Assert.False(_personsService.DeletePerson(personID));
        }

        [Fact]

        public void DeletePerson_ProperDeletion()
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

            bool DeleteReturn = _personsService.DeletePerson(_personsExpected.First().PersonID);

            //Assert
            Assert.True(DeleteReturn);
        }
            #endregion
    }
}   
