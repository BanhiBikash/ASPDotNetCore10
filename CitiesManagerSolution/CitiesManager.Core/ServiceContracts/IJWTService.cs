using CitiesManager.Core.DTO;
using CitiesManager.Core.Entities.IdentityUser;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.Core.ServiceContracts
{
    public interface IJWTService
    {
        AuthResponseDTO CreateJWTToken(ApplicationUser applicationUser);

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}
