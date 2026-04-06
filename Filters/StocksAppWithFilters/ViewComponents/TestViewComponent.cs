using Microsoft.AspNetCore.Mvc;

namespace StocksAppWithFilters.ViewComponents
{
    public class TestViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
