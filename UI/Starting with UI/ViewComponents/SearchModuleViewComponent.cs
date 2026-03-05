using Microsoft.AspNetCore.Mvc;

namespace Starting_with_UI.ViewComponents
{
    public class SearchModuleViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
