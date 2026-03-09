using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IPersonServices
    {
        PersonResponse AddPerson( PersonAddRequests? personAddRequest);
        List<PersonResponse> GetAllPersonResponseList();
        List<PersonResponse> GetFilteredPersons(string? ByProperty, string? PropertyValue);
        List<PersonResponse> GetSortedPersons(string? ByProperty, bool ascending = true);
        bool EditPerson(PersonResponse? personResponse);
        bool DeletePerson(Guid? id);
    }
}
