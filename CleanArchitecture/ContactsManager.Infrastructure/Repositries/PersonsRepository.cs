using RespositoryContract;
using Microsoft.EntityFrameworkCore;
using Entities;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Repositories
{
    public class PersonsRepository : IPersonsRespository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonsRepository> _logger;

        public PersonsRepository(ApplicationDbContext context, ILogger<PersonsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Person> AddPerson(Person person)
        {
            await _context.PersonsData.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid? PersonID)
        {
            var person = await _context.PersonsData.FirstOrDefaultAsync(p => p.PersonID == PersonID);
            if (person != null)
            {
                _context.PersonsData.Remove(person);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<List<Person>?> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons of PersonsRepository");
            return await _context.PersonsData.Include(p => p.Country).ToListAsync();
        }

        public async Task<List<Person>?> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogInformation("GetFilteredPersons of PersonsRepository");

            return await _context.PersonsData.Where(predicate).Include(p => p.Country).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid? PersonID)
        {
            return await _context.PersonsData.FirstOrDefaultAsync(p => p.PersonID == PersonID);
        }

        public async Task<Person?> UpdatePersonByPersonID(Person? Person)
        {
            if (Person == null) return null;

            var personMatching = await _context.PersonsData.FirstOrDefaultAsync(p => p.PersonID == Person.PersonID);
            if (personMatching != null)
            {
                personMatching.PersonName = Person.PersonName;
                personMatching.Email = Person.Email;
                personMatching.DateOfBirth = Person.DateOfBirth;
                personMatching.Gender = Person.Gender;
                personMatching.CountryID = Person.CountryID;
                personMatching.Address = Person.Address;
                personMatching.ReceiveNewsLetters = Person.ReceiveNewsLetters;
                personMatching.TIN = Person.TIN;

                await _context.SaveChangesAsync();
                return personMatching;
            }
            else
            {
                return Person;
            }
        }
    }
}
