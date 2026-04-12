using ContactsManager.Domain.Entities;
using ContactsManager.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using ContactsManager.Infrastructure.DBContext;

namespace ContactsManager.Infrastructure.Repositories
{
    public class CountriesRespository : ICountriesRespository
    {

        private readonly ApplicationDbContext _context;

        public CountriesRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return country;
        }

        public async Task<List<Country>?> GetAllCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryID(Guid? CountryID)
        {
            return await _context.Countries.FirstOrDefaultAsync(country => country.CountryID == CountryID);
        }

        public async Task<Country?> GetCountryByCountryName(string? countryName)
        {
            return await _context.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }
    }
}
