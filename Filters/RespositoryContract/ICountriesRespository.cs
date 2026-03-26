using Entities;

namespace RespositoryContract
{
    public interface ICountriesRespository
    {
        /// <summary>
        /// Adds a country 
        /// </summary>
        /// <param name="country"></param>
        /// <returns>A country</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of countries</returns>
        Task<List<Country>?> GetAllCountries();

        /// <summary>
        /// Takes a CountryId Guid
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns>Returns the country object</returns>
        Task<Country?> GetCountryByCountryID(Guid? CountryID);

        /// <summary>
        /// Takes a string country name
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>A country</returns>
        Task<Country?> GetCountryByCountryName(string? countryName);
    }
}
