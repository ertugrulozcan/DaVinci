using Ertis.Client.Models;
using Ertis.Client.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ertis.Client.Services
{
    public class AuthenticationMockService : IAuthenticationService
    {
        public bool IsLoggedIn { get; }

        public string Token
        {
            get
            {
                return null;
            }
        }

        public bool Login(string username, string password)
        {
            Thread.Sleep(10000);

            return false;
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            return Task.Run(() => this.Login(username, password));
        }

        public event EventHandler<int> OnLoginSuccess;
        public event EventHandler<string> OnLoginFailed;
    }
}
