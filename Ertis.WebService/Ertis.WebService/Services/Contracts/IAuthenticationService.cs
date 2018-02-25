using System.Security.Claims;
using System.Threading.Tasks;
using Ertis.Core.Data;
using Ertis.WebService.Models;

namespace Ertis.WebService.Services.Contracts
{
    public interface IAuthenticationService
    {
        Result Login(string username, string password, string token, string appID);

		Task<ClaimsIdentity> GetIdentity(string username, string password);
	}
}