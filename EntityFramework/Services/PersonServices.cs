
using CsvHelper;
using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System.Globalization;
using System.Reflection;

namespace Services
{
    public class PersonServices : IPersonServices
    {
        private readonly PersonsDBContext _personsDB;

        public PersonServices(PersonsDBContext personDB)
        {
            //injecting the db context to the service class
            _personsDB = personDB;
        }
        //method to add person
        public async Task<PersonResponse> AddPerson(PersonAddRequests? personAddRequest)
        {
            //if the entire request is null
            if (personAddRequest == null) throw new ArgumentNullException("Add request is null. Adding Failed");

            //if any or many attribte of the request is null
            foreach(var property in typeof(PersonAddRequests).GetProperties())
            {
                if(property.GetValue(personAddRequest) == null) throw new ArgumentException(nameof(property)+" is null. Adding Failed!");
            }

            //Actual adding logic
            Person personToAdd = personAddRequest.ConvertToPerson();
            //assigning person id
            personToAdd.PersonID = Guid.NewGuid();

            //Checking current count of person
            int personCount = _personsDB.Persons.Count();

            //adding to the person list
            _personsDB.Persons.Add(personToAdd);
            await _personsDB.SaveChangesAsync();

            //checking if addition is successfull that is count increased
            if (_personsDB.Persons.Count() > personCount)//successfull addition
                {
                    return personToAdd.ToPersonResponse();
                }
                else//addition failed
                {
                    return null;
                }

            //using stored procedure for adding person and checking the result returned by the stored procedure to check if addition is successfully

            //int rowsAffected = _personsDB.InsertPerson(personToAdd);

            //if(rowsAffected > 0) //successfull addition
            //{
            //    return personToAdd.ToPersonResponse();
            //}
            //else //addition failed
            //{
            //    return null;
            //}
        }

        public bool DeletePerson(Guid? id)
        {
            if(id == null) throw new ArgumentNullException("Person ID is null. Deletion Failed");          

            Person? personToDelete = _personsDB.Persons.FirstOrDefault(person => person.PersonID == id);

            if(personToDelete == null) throw new ArgumentException("Person with the given ID does not exist. Deletion Failed");

            //int initialCount = _personsDB.Persons.Count();

            //_personsDB.Persons.Remove(personToDelete);
            //_personsDB.SaveChanges();

            //int finalCount = _personsDB.Persons.Count();

            //if (initialCount>finalCount) //successfull deletion
            //{
            //    return true;
            //}
            //else //deletion failed
            //{
            //    return false;
            //}

            int rowAffected = _personsDB.DeletePerson(id);

            if(rowAffected > 0) //successfull deletion
            {
                return true;
            }
            else //deletion failed
            {
                return false;
            }
        }

        public bool EditPerson(PersonResponse? personResponse)
        {
            if (personResponse == null) throw new ArgumentNullException("Edit request is null. Editing Failed");

            foreach (var property in typeof(PersonResponse).GetProperties())
            {
                if (property.GetValue(personResponse) == null)
                    throw new ArgumentException(nameof(property) + "is null.");
            }

            //List<PersonResponse>? personToEdit = GetFilteredPersons("PersonID", personResponse.PersonID.ToString());

            //Person? existingPerson = _personsDB.Persons.FirstOrDefault(person => person.PersonID == personToEdit[0].PersonID);

            //existingPerson.PersonName = personResponse.PersonName;
            //existingPerson.Email = personResponse.Email;
            //existingPerson.DateOfBirth = personResponse.DateOfBirth;
            //existingPerson.Gender = personResponse.Gender;
            //existingPerson.CountryID = personResponse.CountryID;
            //existingPerson.Address = personResponse.Address;

            //PersonResponse updatedPersonResponse = existingPerson.ToPersonResponse();
            //_personsDB.SaveChanges();

            ////check if the changes took place
            //if (personResponse.Equals(updatedPersonResponse))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}


            //using stored procedure for editing person and checking the result returned by the stored procedure to check if editing is successfully
            Person personToUpdate = new Person() 
            { 
                PersonID = personResponse.PersonID, 
                PersonName = personResponse.PersonName,
                Email = personResponse.Email,
                DateOfBirth = personResponse.DateOfBirth,
                Gender = personResponse.Gender,
                Address = personResponse.Address,
                CountryID = personResponse.CountryID
            };

            int rowsAffected = _personsDB.EditPerson(personToUpdate);

            if(rowsAffected > 0) //successfull editing
            {
                return true;
            }
            else //editing failed
            {
                return false;
            }
        }

        //return all person
        public List<PersonResponse> GetAllPersonResponseList() 
        { 
            List<PersonResponse> personResponses = new List<PersonResponse>();

            foreach(Person person in _personsDB.getAllPersons())
            {
                personResponses.Add(person.ToPersonResponse());
            }

            return personResponses;
        }

        //return person on the basis of filter
        public List<PersonResponse> GetFilteredPersons(string? ByProperty, string? PropertyValue)
        {
            //if either of the filter parameter is null then return all person
            if (string.IsNullOrEmpty(ByProperty) || string.IsNullOrEmpty(PropertyValue))return GetAllPersonResponseList();

            List<string> PersonProperties = new List<string>();

            foreach (var property in typeof(Person).GetProperties())
            {
                PersonProperties.Add(property.Name);
            }

            if (!PersonProperties.Contains(ByProperty))
            {
                throw new ArgumentException("Property Name is invalid");
            }

            //switch (ByProperty.ToLower())
            //{
            //    case "personname":
            //        return GetAllPersonResponseList().FindAll(p => p.PersonName != null && p.PersonName.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
            //    case "email":
            //        return GetAllPersonResponseList().FindAll(p => p.Email != null && p.Email.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
            //    case "address":
            //        return GetAllPersonResponseList().FindAll(p => p.Address != null && p.Address.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
            //    case "personid":
            //        return GetAllPersonResponseList().FindAll(p => p.PersonID != null && p.PersonID.ToString().Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
            //    case "countryid":
            //        return GetAllPersonResponseList().FindAll(p => p.CountryID != null && p.CountryID.Contains(PropertyValue, StringComparison.OrdinalIgnoreCase));
            //    case "dateofbirth":
            //        return GetAllPersonResponseList().FindAll(p => p.DateOfBirth != null && p.DateOfBirth.ToString().Contains(PropertyValue));
            //    case "gender":
            //        return GetAllPersonResponseList().FindAll(p => p.Gender != null && p.Gender.ToString().ToLower().Equals(PropertyValue.ToLower()));

            //    default: return GetAllPersonResponseList();
            //}

            return _personsDB.FilteredPersons(ByProperty, PropertyValue).Select(person => person.ToPersonResponse()).ToList();  
        }

        public async Task<MemoryStream> GetPersonCSV()
        {
            MemoryStream memoryStream = new MemoryStream(); 
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>();
            //PersonId,PersonName,

            csvWriter.NextRecord();
            List<PersonResponse> personResponses = GetAllPersonResponseList();
            await csvWriter.WriteRecordsAsync(personResponses);
            //0000-000,Bob

            await csvWriter.FlushAsync();
            await streamWriter.FlushAsync();

            if (memoryStream.Length == 0) throw new Exception("EMpty stream");

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            ExcelPackage.License.SetNonCommercialPersonal("Banhi");
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream)) 
            { 
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Persons");
                //Adding Headers
                worksheet.Cells[1, 1].Value = "PersonID";
                worksheet.Cells[1, 2].Value = "PersonName";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "DateOfBirth";
                worksheet.Cells[1, 5].Value = "Gender";
                worksheet.Cells[1, 6].Value = "Country";
                worksheet.Cells[1, 7].Value = "GenderKey";   // int codes
                worksheet.Cells[1, 8].Value = "PinCode";     // int 100000–999999

                int row = 2;

                List<PersonResponse> personsList = GetAllPersonResponseList();

                foreach (PersonResponse person in personsList)
                {
                    worksheet.Cells[row, 1].Value = person.PersonID;
                    worksheet.Cells[row, 2].Value = person.PersonName;
                    worksheet.Cells[row, 3].Value = person.Email;
                    worksheet.Cells[row, 4].Value = person.DateOfBirth;
                    worksheet.Cells[row, 5].Value = person.Gender;
                    worksheet.Cells[row, 6].Value = person.CountryID;
                    worksheet.Cells[row, 7].Value = person.GenderKey;
                    worksheet.Cells[row, 8].Value = person.Pin;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsAsync(memoryStream);
            }
            if (memoryStream.Length == 0) throw new Exception("Empty stream");
 
            memoryStream.Position = 0;

            return memoryStream;
        }

        //return person on the basis of sorting
        public List<PersonResponse> GetSortedPersons(string? ByProperty, bool ascending = true)
        {
            if (string.IsNullOrEmpty(ByProperty))
            {
                throw new ArgumentNullException("Property Name cannot be null or empty.", nameof(ByProperty));
            }

            //Getting property info of the property name provided in the parameter
            var propInfo = typeof(PersonResponse).GetProperty(ByProperty,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null)
            {
                throw new ArgumentException($"Property '{ByProperty}' not found on PersonResponse.");
            }


                                //ascending order                                                                                   descending order
            return ascending ? _personsDB.SortPersonsByProperty(ByProperty).Select(person=>person.ToPersonResponse()).ToList() : GetAllPersonResponseList().OrderByDescending(p => propInfo.GetValue(p, null)).ToList();
        }

        public async Task<int> UploadPersonsList(IFormFile formFile)
        {
            MemoryStream memorystream = new MemoryStream();
            await formFile.CopyToAsync(memorystream);

            int personsInserted = 0;
            List<PersonAddRequests>? personList = new List<PersonAddRequests>();

            //setting the license
            ExcelPackage.License.SetNonCommercialPersonal("Banhi");

            using (ExcelPackage excel = new ExcelPackage(memorystream))
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets["Persons"];
                int entries = worksheet.Dimension.Rows;

                for (int row = 2; row <= entries; row++)
                {
                    var person = new PersonAddRequests
                    {
                        PersonName = worksheet.Cells[row, 1].Value?.ToString(),
                        Email = worksheet.Cells[row, 2].Value?.ToString(),
                        Address = worksheet.Cells[row, 6].Value?.ToString(),
                        CountryID = worksheet.Cells[row, 7].Value?.ToString()
                    };

                    // DateOfBirth
                    var dobCell = worksheet.Cells[row, 3].Value;
                    if (dobCell != null)
                    {
                        if (DateTime.TryParse(dobCell.ToString(), out DateTime dob))
                            person.DateOfBirth = dob;
                        else
                            person.DateOfBirth = DateTime.FromOADate(Convert.ToDouble(dobCell));
                    }

                    // Gender
                    if (Enum.TryParse<GenderValues>(worksheet.Cells[row, 4].Value?.ToString(), out var gender))
                        person.Gender = gender;

                    // GenderKey
                    if (int.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out int genderKey))
                        person.GenderKey = genderKey;

                    // Pin
                    if (int.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out int pin))
                        person.Pin = pin;

                    // ✅ Validation: skip if required fields are missing
                    if (string.IsNullOrWhiteSpace(person.PersonName) ||
                        string.IsNullOrWhiteSpace(person.Email) ||
                        !person.DateOfBirth.HasValue ||
                        person.Gender == null ||
                        string.IsNullOrWhiteSpace(person.CountryID) ||
                        !person.Pin.HasValue ||
                        person.Pin < 100000 || person.Pin > 999999)
                    {
                        Console.WriteLine($"Skipping row {row}: invalid or missing data.");
                        continue;
                    }

                    try
                    {
                        // Insert
                        PersonResponse personAdded = await AddPerson(person);
                        await _personsDB.SaveChangesAsync();

                        // Verify insert
                        PersonResponse? check = GetFilteredPersons("PersonID", personAdded.PersonID.ToString()).FirstOrDefault();
                        if (check != null && check.PersonID == personAdded.PersonID)
                        {
                            personsInserted++;
                        }
                        else
                        {
                            Console.WriteLine($"Row {row}: Insert verification failed for {person.PersonName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Row {row}: Failed to add {person.PersonName}. Error: {ex.Message}");
                        continue;
                    }
                }

            }

            return personsInserted;
        }
    }
}
