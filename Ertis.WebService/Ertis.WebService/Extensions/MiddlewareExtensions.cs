using Ertis.WebService.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ertis.WebService.Extensions
{
    public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseTokenProvider(this IApplicationBuilder builder, TokenProviderOptions parameters)
		{
            return builder.UseMiddleware<TokenProviderMiddleware>(Options.Create(parameters));
		}
	}
}