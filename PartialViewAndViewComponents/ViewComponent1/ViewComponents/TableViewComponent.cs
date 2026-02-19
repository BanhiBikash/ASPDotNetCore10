using Microsoft.AspNetCore.Mvc;
using ViewComponent1.Models;

namespace ViewComponent1.ViewComponents
{
    public class TableViewComponent:ViewComponent
    {
        List<City> Cities  =new List<City>()
        {
            new City(){CityUniqueCode="NYC",CityName="New York City",DateAndTime=DateTime.Now,TemperatureFahrenheit=75},
            new City(){CityUniqueCode="LDN",CityName="London",DateAndTime=DateTime.Now,TemperatureFahrenheit=65},
            new City(){CityUniqueCode="TKY",CityName="Tokyo",DateAndTime=DateTime.Now,TemperatureFahrenheit=80},
            new City(){CityUniqueCode="SYD",CityName="Sydney",DateAndTime=DateTime.Now,TemperatureFahrenheit=70}
        };

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(Cities);
        }
    }
}
