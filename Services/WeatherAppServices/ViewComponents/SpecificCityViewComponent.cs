using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
namespace WeatherAppServices.ViewComponents
{
    public class SpecificCityViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(City CityData)
        {
            return View(CityData);
        }
    }
}
