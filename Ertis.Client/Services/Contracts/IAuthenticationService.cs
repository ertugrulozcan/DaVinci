using Ertis.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Client.Services.Contracts
{
    public interface IAuthenticationService
    {
        bool IsLoggedIn { get; }

        string Token { get; }

        bool Login(string username, string password);

        Task<bool> LoginAsync(string username, string password);

        event EventHandler<int> OnLoginSuccess;
        event EventHandler<string> OnLoginFailed;
    }
}
