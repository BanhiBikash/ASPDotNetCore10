using Microsoft.AspNetCore.Mvc;
using WeatherAppViewComponents.Models;

namespace WeatherAppViewComponents.ViewComponents
{
    public class SpecificCityViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string cityUniqueCode)
        {
            List<City> cities = new List<City>()
            {
                new City() { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = DateTime.Now, TemperatureFahrenheit = 75 },
                new City() { CityUniqueCode = "LA", CityName = "Los Angeles", DateAndTime = DateTime.Now, TemperatureFahrenheit = 85 },
                new City() { CityUniqueCode = "CHI", CityName = "Chicago", DateAndTime = DateTime.Now, TemperatureFahrenheit = 70 },
                new City() { CityUniqueCode = "HOU", CityName = "Houston", DateAndTime = DateTime.Now, TemperatureFahrenheit = 90 },
                new City() { CityUniqueCode = "PHX", CityName = "Phoenix", DateAndTime = DateTime.Now, TemperatureFahrenheit = 100 }
            };

            City? Data = cities.Where(c=>c.CityUniqueCode == cityUniqueCode).FirstOrDefault();   

            return View(Data);
        }
    }
}
