using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using System.Text;

namespace Ertis.WebService.Auth
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public DateTime NotBefore => DateTime.UtcNow;

        public DateTime IssuedAt => DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }

        public JwtIssuerOptions(JWTSettings settings)
        {
            this.Issuer = settings.Issuer;
            this.Audience = settings.Audience;
            this.ValidFor = settings.Expiration;
            this.SigningCredentials = Helpers.SignatureHelper.GenerateSigningCredentials(settings.SecretKey);
        }
    }
}