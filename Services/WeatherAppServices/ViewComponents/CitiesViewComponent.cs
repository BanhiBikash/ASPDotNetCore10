using Microsoft.AspNetCore.Mvc;
using WeatherAppServices.Models;
using Services;

namespace WeatherAppServices.ViewComponents
{
    public class CitiesViewComponent: ViewComponent
    {
        //public List<string> Cities;
        //public CitiesViewComponent()
        //{
        //    CitiesService citiesService = new CitiesService();
        //    Cities = citiesService.GetCities();
        //}

        public async Task<IViewComponentResult> InvokeAsync(List<string> Cities)
        {
            return View(Cities);
        }
    }
}
