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
        [Route("/sortBy/{sortBy}")]
        public IActionResult Index(string? searchBy, string? searchValue, string? sortBy)
        {
            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(sortBy))
            {
                return View(_personsService.GetAllPersonResponseList());
            }

            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchValue))
            {
                ViewBag.SearchBy = searchBy;
                ViewBag.SearchValue = searchValue;
                return View(_personsService.GetFilteredPersons(searchBy, searchValue));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                return View(_personsService.GetSortedPersons(sortBy));
            }

            // fallback
            return View(_personsService.GetAllPersonResponseList());
        }
    }
}
