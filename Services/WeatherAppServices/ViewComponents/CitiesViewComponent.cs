using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace WeatherAppServices.ViewComponents
{
    public class CitiesViewComponent: ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(List<City> Cities)
        {
            return View(Cities);
        }
    }
}
