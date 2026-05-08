using System;
using System.Collections.Generic;
using System.Text;

namespace CitiesManager.Core.DTO
{
    public class AuthResponseDTO
    {
        public string? PersonName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string? Token { get; set; } = string.Empty;

        public string? RefreshToken { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
