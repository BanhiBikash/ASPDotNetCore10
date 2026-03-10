using Microsoft.AspNetCore.Mvc;

namespace EntityFramework1.ViewComponents
{
    public class SearchModuleViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
