using System;
using Microsoft.IdentityModel.Tokens;

namespace Ertis.WebService.Auth
{
    public class JWTSettings
    {
        public string Path { get; set; } = "/api/login";

        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Authority { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(3600);

        public string CookieName { get; set; }
    }
}