using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Core.Entities.IdentityUser
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

        public string? RefreshToken { get; set; } = string.Empty;

        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
