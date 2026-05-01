using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Core.Entities.IdentityUser
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
