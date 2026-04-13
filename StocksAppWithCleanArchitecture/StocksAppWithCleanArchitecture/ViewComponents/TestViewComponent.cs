using Microsoft.AspNetCore.Mvc;

namespace StocksApp.UI.ViewComponents
{
    public class TestViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
