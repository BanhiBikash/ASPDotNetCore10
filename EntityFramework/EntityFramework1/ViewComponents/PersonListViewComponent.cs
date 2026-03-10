using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTO;

namespace EntityFramework1.ViewComponents
{
    public class PersonListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<PersonResponse> personResponses)
        {
            return View(personResponses);
        }
    }
}
