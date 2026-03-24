using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services.Helpers;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RespositoryContract;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Logging;

namespace Services
{
 public class PersonsService : IPersonsService
 {
  //private field
  private readonly IPersonsRespository _personRepository;
  private readonly ICountriesRespository _countryRespository;
        private readonly ILogger<PersonsService> _logger;

  //constructor
  public PersonsService(IPersonsRespository personsRespository, ILogger<PersonsService> logger)
  {
   _personRepository = personsRespository;
   _logger = logger;
   //_countryRespository = countriesRespository;
  }


  public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
  {
   //check if PersonAddRequest is not null
   if (personAddRequest == null)
   {
    throw new ArgumentNullException(nameof(personAddRequest));
   }

   //Model validation
   ValidationHelper.ModelValidation(personAddRequest);

   //convert personAddRequest into Person type
   Person person = personAddRequest.ToPerson();

   //generate PersonID
   person.PersonID = Guid.NewGuid();

   //add person object to persons list
   Person personAdded = await _personRepository.AddPerson(person);
   //_db.sp_InsertPerson(person);

   //convert the Person object into PersonResponse type
   return personAdded!=null?personAdded.ToPersonResponse():null;
  }


  public async Task<List<PersonResponse>> GetAllPersons()
  {
            _logger.LogInformation("GetAllPersons of Persons Service");
   //SELECT * from Persons
   var persons = await _personRepository.GetAllPersons();

   return persons
     .Select(temp => temp.ToPersonResponse()).ToList();

   //return _db.sp_GetAllPersons()
   //  .Select(temp => temp.ToPersonResponse()).ToList();
  }


  public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
  {
   if (personID == null)
    return null;

            Person? person = await _personRepository.GetPersonByPersonID(personID);

   if (person == null)
    return null;

   return person.ToPersonResponse();
  }


  public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
  {
            _logger.LogInformation("GetFilteredPersons of Persons Service");
            _logger.LogDebug($"GetFilteredPersons of Persons Service, searchBy:{searchBy}, searchString: {searchString}");
            List<Person> filteredPersons = searchBy switch
            {
                nameof(PersonResponse.PersonName) => await _personRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(searchString) || (temp.PersonName != null && temp.PersonName.Contains(searchString))),

                nameof(PersonResponse.Email) => await _personRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(searchString) || (temp.Email != null && temp.Email.Contains(searchString))),

                nameof(PersonResponse.DateOfBirth) => await _personRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(searchString) || (temp.DateOfBirth != null && temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString))),

                nameof(PersonResponse.Gender) => await _personRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(searchString) || (temp.Gender != null && temp.Gender.Contains(searchString))),

                nameof(PersonResponse.CountryID) => await _personRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(searchString) || (temp.Country != null && temp.Country.CountryName.Contains(searchString))),

                nameof(PersonResponse.Address) => await _personRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(searchString) || (temp.Address != null && temp.Address.Contains(searchString))),

                _ => await _personRepository.GetAllPersons()
            };

      return filteredPersons.Select(temp => temp.ToPersonResponse()).ToList();
}


        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
  {
            _logger.LogInformation("GetSortedPersons of PersonsService");
            _logger.LogDebug($"GetSortedPersons of Persons Service sortBy:{sortBy} and SortOrderOptions:{sortOrder}");
   if (string.IsNullOrEmpty(sortBy))
    return allPersons;

   List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
   {
    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

    (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

    (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

    (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

    _ => allPersons
   };

   return sortedPersons;
  }


  public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
  {
            _logger.LogInformation("Update Person of Persons Service.");

   if (personUpdateRequest == null)
    throw new ArgumentNullException(nameof(Person));

   //validation
   ValidationHelper.ModelValidation(personUpdateRequest);

   //get matching person object to update
   Person? matchingPerson = await _personRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
   if (matchingPerson == null)
   {
    throw new ArgumentException("Given person id doesn't exist");
   }

   matchingPerson = await _personRepository.UpdatePersonByPersonID(personUpdateRequest.ToPerson()); //UPDATE

   return matchingPerson.ToPersonResponse();
  }

  public async Task<bool> DeletePerson(Guid? personID)
  {

            _logger.LogInformation("DeletePerson of PersonsService");

   if (personID == null)
   {
                _logger.LogError("DeletePerson of Persons Service: PesonnId is null.");
    throw new ArgumentNullException(nameof(personID));
   }

   return await _personRepository.DeletePersonByPersonID(personID);
  }

  public async Task<MemoryStream> GetPersonsCSV()
  {
   MemoryStream memoryStream = new MemoryStream();
   StreamWriter streamWriter = new StreamWriter(memoryStream);

   CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
   CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

   //PersonName,Email,DateOfBirth,Age,Gender,Country,Address,ReceiveNewsLetters
   csvWriter.WriteField(nameof(PersonResponse.PersonName));
   csvWriter.WriteField(nameof(PersonResponse.Email));
   csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
   csvWriter.WriteField(nameof(PersonResponse.Age));
   csvWriter.WriteField(nameof(PersonResponse.Country));
   csvWriter.WriteField(nameof(PersonResponse.Address));
   csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
   csvWriter.NextRecord();

   List<PersonResponse> persons = (await _personRepository.GetAllPersons()).Select(temp=>temp.ToPersonResponse()).ToList();

   foreach (PersonResponse person in persons)
   {
    csvWriter.WriteField(person.PersonName);
    csvWriter.WriteField(person.Email);
    if (person.DateOfBirth.HasValue)
     csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
    else
     csvWriter.WriteField("");
    csvWriter.WriteField(person.Age);
    csvWriter.WriteField(person.Country);
    csvWriter.WriteField(person.Address);
    csvWriter.WriteField(person.ReceiveNewsLetters);
    csvWriter.NextRecord();
    csvWriter.Flush();
   }

   memoryStream.Position = 0;
   return memoryStream;
  }

  public async Task<MemoryStream> GetPersonsExcel()
  {
   MemoryStream memoryStream = new MemoryStream();
   using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
    workSheet.Cells["A1"].Value = "Person Name";
    workSheet.Cells["B1"].Value = "Email";
    workSheet.Cells["C1"].Value = "Date of Birth";
    workSheet.Cells["D1"].Value = "Age";
    workSheet.Cells["E1"].Value = "Gender";
    workSheet.Cells["F1"].Value = "Country";
    workSheet.Cells["G1"].Value = "Address";
    workSheet.Cells["H1"].Value = "Receive News Letters";

    using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
    {
     headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
     headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
     headerCells.Style.Font.Bold = true;
    }

    int row = 2;
                List<PersonResponse> persons = (await _personRepository.GetAllPersons()).Select(temp => temp.ToPersonResponse()).ToList();
    foreach (PersonResponse person in persons)
    {
     workSheet.Cells[row, 1].Value = person.PersonName;
     workSheet.Cells[row, 2].Value = person.Email;
     if (person.DateOfBirth.HasValue)
      workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
     workSheet.Cells[row, 4].Value = person.Age;
     workSheet.Cells[row, 5].Value = person.Gender;
     workSheet.Cells[row, 6].Value = person.Country;
     workSheet.Cells[row, 7].Value = person.Address;
     workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

     row++;
    }

    workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

    await excelPackage.SaveAsync();
   }

   memoryStream.Position = 0;
   return memoryStream;
  }
 }
}
