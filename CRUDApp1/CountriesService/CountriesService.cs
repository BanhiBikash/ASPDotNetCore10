using ServiceContracts;
using Entities;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private List<Country>? _countries;
        private List<CountryResponse> _countriesList;

        public CountriesService() 
        { 
            _countries = new List<Country>();
            _countriesList = new List<CountryResponse>();

        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: Country Add request can't be null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException("Country Request is null",nameof(countryAddRequest));
            }

            //Validation: CountryName in null
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException("Country name is null.",nameof(countryAddRequest.CountryName));
            }

            //Validation: Duplicate Country Name is not allowed
            if (_countries.Any((country) => country.CountryName == countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country already exists.",nameof(countryAddRequest.CountryName));
            }

            Country? country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _countries.Add(country);

            //CountryExtension countryExtension = new CountryExtension();

            //returning the value
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetCountryList()
        {
            foreach (Country country in _countries) { 
                _countriesList.Add(country.ToCountryResponse());
            }

            return _countriesList;
        }

        public CountryResponse GetCountryByCountryID(Guid CountryID)
        {
            if(CountryID == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            foreach (Country country in _countries)
            {
                _countriesList.Add(country.ToCountryResponse());
            }

            return _countriesList.Where(c => c.CountryID == CountryID).FirstOrDefault();
        }
    }
}
