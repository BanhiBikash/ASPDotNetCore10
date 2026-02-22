using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts
{
    public interface ISpecificCity
    {
        City GetCityDetails(string CityCode);
    }
}
