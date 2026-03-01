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
        #region AddCountry

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

        #endregion

        #region Country_List

        //shoud get empty country repsonse list
        [Fact]
        public void CountryList__shouldBeEmpty()
        {
            //Arrange
            //no arrangement required

            //Act
            List<CountryResponse> emptyCountryList = _countriesService.GetCountryList();

            //Assert
            Assert.Empty(emptyCountryList);
        }

        //Get the countries added back
        [Fact]
        public void CountryList_getSameCountryBack()
        {
            //Arrange
            List<CountryAddRequest> Brics = new List<CountryAddRequest>() 
            {
                new CountryAddRequest() { CountryName = "Brazil" },
                new CountryAddRequest() { CountryName = "Russia"},
                new CountryAddRequest() { CountryName = "India"},
                new CountryAddRequest() { CountryName = "China"},
                new CountryAddRequest() { CountryName = "South Africa"}
            };

            List<CountryResponse> countryResponses = new List<CountryResponse>();

            foreach(var country in Brics)
            {
                countryResponses.Add(_countriesService.AddCountry(country)); 
            }

            //Act
            List<CountryResponse> responseList = _countriesService.GetCountryList();

            //Assert
            //bool Match = true;

            //for (int i = 0; i < countryResponses.Count; i++)
            //{
            //    if (!responseList.Contains(countryResponses[i]))
            //    {
            //        Match = false;
            //        break;
            //    }
            //}

            //Assert.True(Match);

            foreach (CountryResponse cr in responseList)
            {
                Assert.Contains(cr, countryResponses);
            }

        }

        #endregion

        #region CountryId

        [Fact]
        public void GetCountryByCountryID_NullArgument()
        {
            //Arrange
            Guid CountryID = Guid.Empty;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.GetCountryByCountryID(CountryID);
            });
        }


        [Fact]
        public void GetCountryBack()
        {
            //Arrange
            List<CountryAddRequest> countryAddRequestList = new List<CountryAddRequest>() { 
                new CountryAddRequest(){CountryName = "Japan"},
                new CountryAddRequest(){CountryName = "China"},
                new CountryAddRequest(){CountryName = "India"}
            };

            List<CountryResponse> countryResponses = new List<CountryResponse>();

            foreach (CountryAddRequest countryAddRequest in countryAddRequestList)
            {
                countryResponses.Add(_countriesService.AddCountry(countryAddRequest));
            }

            //Act
            //CountryResponse Result  = _countriesService.GetCountryByCountryID(countryResponses[0].CountryID);

            //Act and Assert 
            foreach(CountryResponse cr in countryResponses)
            {
                CountryResponse Result = _countriesService.GetCountryByCountryID(cr.CountryID);
                Assert.Equal(cr, Result);
            }
        }
        #endregion

    }
}
