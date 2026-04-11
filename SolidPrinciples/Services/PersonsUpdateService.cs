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
using Exceptions;

namespace Services
{
 public class PersonsUpdateService : IPersonsUpdateService
 {
  //private field
  private readonly IPersonsRespository _personRepository;
  private readonly ICountriesRespository _countryRespository;
        private readonly ILogger<PersonsSortService> _logger;

  //constructor
  public PersonsUpdateService(IPersonsRespository personsRespository, ILogger<PersonsSortService> logger)
  {
   _personRepository = personsRespository;
   _logger = logger;
   //_countryRespository = countriesRespository;
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
        throw new InvalidPersonIDException("Given person id doesn't exist");
   }

   matchingPerson = await _personRepository.UpdatePersonByPersonID(personUpdateRequest.ToPerson()); //UPDATE

   return matchingPerson.ToPersonResponse();
  }
 }
}
