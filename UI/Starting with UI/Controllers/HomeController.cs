using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Services;
using Starting_with_UI.Models;

namespace Starting_with_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonServices _personsService;
        public HomeController(IPersonServices personServices)
        {
            _personsService = personServices;
            ViewBag.sortByASC = null;
        }

        [Route("/")]
        [Route("/sortBy/{sortBy}/{sortByASC:bool}")]
        public IActionResult Index(string? searchBy, string? searchValue, string? sortBy, bool? sortByASC)
        {
            bool ascending = sortByASC ?? true; // default to ascending if null
            ViewBag.sortByASC = ascending;

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
                return View(_personsService.GetSortedPersons(sortBy,ascending));
            }

            // fallback
            return View(_personsService.GetAllPersonResponseList());
        }

        [Route("/AddPerson")]
        public IActionResult CreateNewPerson() 
        {
            return View();
        }

        [Route("/persons/create")]
        public IActionResult AddingPerson(PersonAddRequests? personAddRequest)
        {
            List<string> errors = null;

            if (!ModelState.IsValid)
            {
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(); 
                return View("CreateNewPerson",errors);
            }

            ErrorListAndPersonRequest pes = new ErrorListAndPersonRequest
            {
                Errors = errors,
                PersonAddRequest = personAddRequest
            };

            _personsService.AddPerson(pes);

            return View("Index", _personsService.GetAllPersonResponseList());
        }
    }
}
