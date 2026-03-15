using ServicesContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServicesContracts
{
    public interface IPersonServices
    {
        Task<PersonResponse> AddPerson( PersonAddRequests? personAddRequest);
        List<PersonResponse> GetAllPersonResponseList();
        List<PersonResponse> GetFilteredPersons(string? ByProperty, string? PropertyValue);
        List<PersonResponse> GetSortedPersons(string? ByProperty, bool ascending = true);
        bool EditPerson(PersonResponse? personResponse);
        bool DeletePerson(Guid? id);
        Task<MemoryStream> GetPersonCSV();
        Task<MemoryStream> GetPersonExcel();
        Task<int> UploadPersonsList(IFormFile formFile);
    }
}
