using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.AuthorizationFilters;
using ContactsManager.UI.Filters.ResultFilters;
using ContactsManager.UI.Filters.SkipFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts.Enums;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.UI.Controllers
{
    [TypeFilter(typeof(ResponseHeaderFilter), Arguments = new object[]{"PesonControllerKey", "PesonControllerValue",3 })]
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsSortService _personsSortService;
        private readonly IPersonsAddService _personsAddService;
        private readonly IPersonsDeleteService _personsDeleteService;
        private readonly IPersonsGetService _personsGetService;
        private readonly IPersonsUpdateService _personsUpdateService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        //constructor
        public PersonsController(IPersonsSortService personsSortService, IPersonsAddService personsAddService, IPersonsDeleteService personsDeleteService, IPersonsGetService personsGetService, IPersonsUpdateService personsUpdateService , ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personsSortService = personsSortService;
            _personsAddService = personsAddService;
            _personsDeleteService = personsDeleteService;
            _personsGetService = personsGetService;
            _personsUpdateService = personsUpdateService;
            _countriesService = countriesService;
            _logger = logger;
        }

        //Url: persons/index
        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(IndexActionFilter))]
        [TypeFilter(typeof(ResponseHeaderFilter), Arguments = new object[] { "Key1", "Value1", 1 })]
        [IndexResultFilterAttribute("ResultKey","ResultValue")]
  public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
  {

            _logger.LogInformation("Index Method of Persons Controller");
   //Search
   
           // _logger.LogDebug($"Search parameters Index of Person controller, ID: {ViewBag.SearchFields[nameof(PersonResponse.PersonID)]}, Name: {ViewBag.SearchFields[nameof(PersonResponse.PersonName)]}, Email: {ViewBag.SearchFields[nameof(PersonResponse.Email)]}, DateOfBirth: {ViewBag.SearchFields[nameof(PersonResponse.DateOfBirth)]}, Gender: {ViewBag.SearchFields[nameof(PersonResponse.Gender)]}, Country: {ViewBag.SearchFields[nameof(PersonResponse.CountryID)]}, Address: {ViewBag.SearchFields[nameof(PersonResponse.Address)]}, ReceiveNewsLetters: {ViewBag.SearchFields[nameof(PersonResponse.ReceiveNewsLetters)]}");

            List<PersonResponse> persons = await _personsGetService.GetFilteredPersons(searchBy, searchString);
   //ViewBag.CurrentSearchBy = searchBy;
   //ViewBag.CurrentSearchString = searchString;

   //Sort
   List<PersonResponse> sortedPersons = await _personsSortService.GetSortedPersons(persons, sortBy, sortOrder);
   //ViewBag.CurrentSortBy = sortBy;
   //ViewBag.CurrentSortOrder = sortOrder.ToString();

   return View(sortedPersons); //Views/Persons/Index.cshtml
  }


  //Executes when the user clicks on "Create Person" hyperlink (while opening the create view)
  //Url: persons/create
  [Route("[action]")]
  [HttpGet]
  [TypeFilter(typeof(ResponseHeaderFilter), Arguments = new object[] { "Key2", "Value2",3 })]
        public async Task<IActionResult> Create()
  {
   List<CountryResponse> countries = await _countriesService.GetAllCountries();
   ViewBag.Countries = countries.Select(temp =>
     new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() }
   );

   //new SelectListItem() { Text="Harsha", Value="1" }
   //<option value="1">Harsha</option>
   return View();
  }

  [HttpPost]
  //Url: persons/create
  [Route("[action]")]
  [TypeFilter(typeof(PersonsCreateandEditFilter))]
        //[TypeFilter(typeof(SkipResponseFilter))]
  public async Task<IActionResult> Create(PersonAddRequest personRequest)
  {

   //call the service method
   PersonResponse personResponse = await _personsAddService.AddPerson(personRequest);

   //navigate to Index() action method (it makes another get request to "persons/index"
   return RedirectToAction("Index", "Persons");
  }

  [HttpGet]
  [Route("[action]/{personID}")] //Eg: /persons/edit/1
  [TypeFilter(typeof(TokenAuthorizationfilter))]
  public async Task<IActionResult> Edit(Guid personID)
  {
   PersonResponse? personResponse = await _personsGetService.GetPersonByPersonID(personID);
   if (personResponse == null)
   {
    return RedirectToAction("Index");
   }

   PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

   List<CountryResponse> countries = await _countriesService.GetAllCountries();
   ViewBag.Countries = countries.Select(temp =>
   new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

   return View(personUpdateRequest);
  }


  [HttpPost]
  [Route("[action]/{personID}")]
  [TypeFilter(typeof(PersonsCreateandEditFilter))]
  public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
  {
   PersonResponse? personResponse = await _personsGetService.GetPersonByPersonID(personRequest.PersonID);

   if (personResponse == null)
   {
    return RedirectToAction("Index");
   }

   if (ModelState.IsValid)
   {
    PersonResponse updatedPerson = await _personsUpdateService.UpdatePerson(personRequest);
    return RedirectToAction("Index");
   }
   else
   {
    //List<CountryResponse> countries = await _countriesService.GetAllCountries();
    //ViewBag.Countries = countries.Select(temp =>
    //new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

    //ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
    return View(personResponse.ToPersonUpdateRequest());
   }
  }


  [HttpGet]
  [Route("[action]/{personID}")]
  public async Task<IActionResult> Delete(Guid? personID)
  {
   PersonResponse? personResponse = await _personsGetService.GetPersonByPersonID(personID);
   if (personResponse == null)
    return RedirectToAction("Index");

   return View(personResponse);
  }

  [HttpPost]
  [Route("[action]/{personID}")]
  public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateResult)
  {
   PersonResponse? personResponse = await _personsGetService.GetPersonByPersonID(personUpdateResult.PersonID);
   if (personResponse == null)
    return RedirectToAction("Index");

   await _personsDeleteService.DeletePerson(personUpdateResult.PersonID);
   return RedirectToAction("Index");
  }


  [Route("PersonsPDF")]
  public async Task<IActionResult> PersonsPDF()
  {
   //Get list of persons
   List<PersonResponse> persons = await _personsGetService.GetAllPersons();

   //Return view as pdf
   return new ViewAsPdf("PersonsPDF", persons, ViewData)
   {
    PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
   };
  }


  [Route("PersonsCSV")]
  public async Task<IActionResult> PersonsCSV()
  {
   MemoryStream memoryStream = await _personsGetService.GetPersonsCSV();
   return File(memoryStream, "application/octet-stream", "persons.csv");
  }


  [Route("PersonsExcel")]
  public async Task<IActionResult> PersonsExcel()
  {
   MemoryStream memoryStream = await _personsGetService.GetPersonsExcel();
   return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
  }
 }
}
