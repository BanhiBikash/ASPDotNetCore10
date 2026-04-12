using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RespositoryContract;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class PersonGetServiceWithNewExcelFormat : IPersonsGetService
    {
        private readonly PersonsGetService _personGetService;
        private readonly IPersonsRespository _personRepository;
        private readonly ILogger<PersonsGetService> _logger;

        public PersonGetServiceWithNewExcelFormat(PersonsGetService personGetService,IPersonsRespository personRepository, ILogger<PersonsGetService> logger)
        {
            _personGetService = personGetService;
            _personRepository = personRepository;
            _logger = logger;
        }
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personGetService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            return await _personGetService.GetFilteredPersons(searchBy, searchString);
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            return await _personGetService.GetPersonByPersonID(personID);
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
          return  await _personGetService.GetPersonsCSV();
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Age";
                workSheet.Cells["C1"].Value = "Gender";

                using (ExcelRange headerCells = workSheet.Cells["A1:C1"])
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
                    workSheet.Cells[row, 2].Value = person.Age;
                    workSheet.Cells[row, 3].Value = person.Gender;

                    row++;
                }

                workSheet.Cells[$"A1:C{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
