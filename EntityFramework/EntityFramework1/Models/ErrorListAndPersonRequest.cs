using ServicesContracts.DTO;

namespace EntityFramework1.Models
{
    public class ErrorListAndPersonRequest
    {
        public List<string>? Errors { get; set; }
        public PersonAddRequests? PersonAddRequest { get; set; }
    }
}
