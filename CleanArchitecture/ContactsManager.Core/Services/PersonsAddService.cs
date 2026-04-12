using System;
using Entities;
using ContactsManager.Core.DTO;
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
using Exceptions;

namespace Services
{
 public class PersonsAddService : IPersonsAddService
 {
  //private field
  private readonly IPersonsRespository _personRepository;
  private readonly ICountriesRespository _countryRespository;
        private readonly ILogger<PersonsAddService> _logger;

  //constructor
  public PersonsAddService(IPersonsRespository personsRespository, ILogger<PersonsAddService> logger)
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

 }
}
