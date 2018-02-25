using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ertis.WebService.Helpers
{
    public static class SignatureHelper
    {
        public static SigningCredentials GenerateSigningCredentials(string secretKey)
        {
			var signingKey = GenerateSigningKey(secretKey);
			var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			return signingCredentials;
		}

        public static SymmetricSecurityKey GenerateSigningKey(string secretKey)
        {
            var keyByteArray = Encoding.ASCII.GetBytes(secretKey);
			var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyByteArray);

			return signingKey;
		}
    }
}