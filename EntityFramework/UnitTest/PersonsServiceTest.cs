using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using Xunit.Sdk;

namespace UnitTest
{
    public class PersonsServiceTest
    {
        private readonly IPersonServices _personService;
        private readonly TestOutputHelper _testOutputHelper;

        public PersonsServiceTest(TestOutputHelper testOutputHelper)
        {
            var personListInitial = new List<Person>() { };


            //creating the mock DB context options
            DbContextMock<PersonsDBContext> dbContextMock = new DbContextMock<PersonsDBContext>(new DbContextOptionsBuilder<PersonsDBContext>().Options);
            
            PersonsDBContext personDBContext = dbContextMock.Object;


            //creating the mock db set
            dbContextMock.CreateDbSetMock(temp=>temp.Persons, personListInitial);

            _personService = new PersonServices(personDBContext);
            _testOutputHelper = testOutputHelper;
        }

        #region PersonAdd

        [Fact]
        public async Task AddPersonArgumentNull()
        {
            //Arrange
            PersonAddRequests person = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _personService.AddPerson(person);
            }
             );
        }

        #endregion
    }
}
