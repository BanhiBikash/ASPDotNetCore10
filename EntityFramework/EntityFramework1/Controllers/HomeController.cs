using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using EntityFramework1.Models;
using Rotativa.AspNetCore;  
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EntityFramework1.Controllers
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
                return View(_personsService.GetSortedPersons(sortBy, ascending));
            }

            // fallback
            return View(_personsService.GetAllPersonResponseList());
        }

        [Route("/AddPerson")]
        public IActionResult CreateNewPerson()
        {

            ErrorListAndPersonRequest pes = new ErrorListAndPersonRequest
            {
                Errors = null,
                PersonAddRequest = null
            };

            return View(pes);
        }

        [Route("/persons/create")]
        public async Task<IActionResult> AddingPerson(PersonAddRequests? personAddRequest)
        {
            List<string> errors = null;

            if (!ModelState.IsValid)
            {
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ErrorListAndPersonRequest pes = new ErrorListAndPersonRequest
                {
                    Errors = errors,
                    PersonAddRequest = personAddRequest
                };

                // Pass the full model back
                return View("CreateNewPerson", pes);
            }

            await _personsService.AddPerson(personAddRequest);

            return View("Index", _personsService.GetAllPersonResponseList());
        }

        [Route("/persons/edit")]
        public IActionResult EditPerson(Guid? id)
        {
            List<PersonResponse> personToEdit = _personsService.GetFilteredPersons("PersonID", id.ToString());

            //person not found
            if (personToEdit.Count == 0) return BadRequest("Person is not found");

            //person found
            return View(personToEdit[0]);
        }

        [HttpPost]
        [Route("/persons/update")]
        public IActionResult UpdatingPerson(PersonResponse? personResponse)
        {
            bool result = _personsService.EditPerson(personResponse);

            if (result)
            {
                return View("Index", _personsService.GetAllPersonResponseList());
            }
            else
            {
                return Content("text/html", "Not Updated");
            }
        }

        [Route("/persons/delete")]
        public IActionResult DeletePerson(Guid? id)
        {
            bool result = _personsService.DeletePerson(id);

            if (result)
            {
                return View("Index", _personsService.GetAllPersonResponseList());
            }
            else
            {
                return Content("text/html", "Not Deleted");
            }
        }

        [Route("/persons/printpersonlist")]
        public IActionResult PrintPersonsList()
        {
            List<PersonResponse>? personList = _personsService.GetAllPersonResponseList();

            return new ViewAsPdf("PersonsPDF", personList, ViewData);
        }

        [Route("persons/PersonsCSV")]
        public async Task<IActionResult> GetPersonsCSV()
        {
            MemoryStream stream = await _personsService.GetPersonsCSV();
            return File(stream, "application/octet-stream", "PersonsList.csv");
        }
    }
}
