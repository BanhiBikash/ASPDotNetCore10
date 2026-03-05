using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTO;
namespace Starting_with_UI.ViewComponents
{
    public class PersonListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<PersonResponse> personResponses)
        {
            return View(personResponses);
        }
    }
}
