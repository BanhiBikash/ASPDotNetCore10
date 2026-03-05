using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IPersonServices
    {
        PersonResponse AddPerson( PersonAddRequests? personAddRequest);


    }
}
