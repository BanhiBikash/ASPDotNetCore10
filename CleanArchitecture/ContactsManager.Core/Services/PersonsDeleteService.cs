using System;
using ContactsManager.Domain.Entities;
using ContactsManager.Core.DTO;
using ServiceContracts;
using Services.Helpers;
using ServiceContracts.Enums;
//using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using OfficeOpenXml;
using ContactsManager.Domain.RepositoryContracts;
//using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Logging;
using ContactsManager.Exceptions;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.Core.Services
{
 public class PersonsDeleteService : IPersonsDeleteService
 {
  //private field
  private readonly IPersonsRespository _personRepository;
  private readonly ICountriesRespository _countryRespository;
        private readonly ILogger<PersonsDeleteService> _logger;

  //constructor
  public PersonsDeleteService(IPersonsRespository personsRespository, ILogger<PersonsDeleteService> logger)
  {
   _personRepository = personsRespository;
   _logger = logger;
   //_countryRespository = countriesRespository;
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
 }
}
