using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Services;

namespace Starting_with_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonServices _personsService;
        public HomeController(IPersonServices personServices)
        {
            _personsService = personServices;
        }

        [Route("/")]
        public IActionResult Index(string? searchBy, string? searchValue)
        {
            //if searchBy is null then return all person otherwise return filtered person
            if (searchBy == null)
            return View(_personsService.GetAllPersonResponseList());

            //if searchBy is not null then return filtered person on the basis of searchBy and searchValue
            ViewBag.SearchBy = searchBy;
            ViewBag.SearchValue = searchValue;
            return View(_personsService.GetFilteredPersons(searchBy,searchValue));
        }
    }
}
