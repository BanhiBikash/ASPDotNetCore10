using ServicesContracts.DTO;

namespace Starting_with_UI.Models
{
    public class ErrorListAndPersonRequest
    {
        public List<string>? Errors { get; set; }
        public PersonAddRequests? PersonAddRequest { get; set; }
    }
}
