using Microsoft.AspNetCore.Mvc;
using ViewComponent1.Models;

namespace ViewComponent1.ViewComponents
{
    public class TableViewComponent:ViewComponent
    {
        //The argument name and the parameter name must be same
        public async Task<IViewComponentResult> InvokeAsync(List<City> Cities)
        {
            return View(Cities);
        }
    }
}
