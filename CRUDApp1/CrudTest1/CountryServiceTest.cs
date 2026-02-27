using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CrudTest1
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;
    
        public CountryServiceTest()
        {
            _countriesService = new CountriesService() { };
        }

        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        [Fact]
        public void AddCountry_CountryNameisNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null};

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        [Fact]
        public void AddCountry_CountryNameisDuplicate()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
                _countriesService.AddCountry(request2);
            });
        }

        [Fact]
        public void AddCountry_CountryNameisProper()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse countryResponse = _countriesService.AddCountry(request);

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
        }
    }
}
