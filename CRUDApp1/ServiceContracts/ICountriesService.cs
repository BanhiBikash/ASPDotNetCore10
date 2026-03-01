using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a new country using the specified country details.
        /// </summary>
        /// <param name="countryAddRequest">An object containing the details of the country to add. Can be null if no country is to be added.</param>
        /// <returns>A CountryResponse object containing the result of the add operation, including information about the newly
        /// added country or any errors encountered.</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns country list
        /// </summary>
        /// <returns></returns>
        List<CountryResponse> GetCountryList();

        /// <summary>
        /// Returns the country by Country ID
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        CountryResponse GetCountryByCountryID(Guid CountryID);
    }
}
