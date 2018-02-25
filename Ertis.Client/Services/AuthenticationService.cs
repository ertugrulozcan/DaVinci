using Ertis.Client.Services.Contracts;
using Ertis.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ertis.Core.Connect;
using Ertis.Client.Models;
using Newtonsoft.Json;
using Ertis.Client.Managers.Contracts;

namespace Ertis.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Services

        private readonly IErtisWebService ertisWebService;
        private readonly ISessionManager sessionManager;

        #endregion

        #region Fields
        
        private bool isLoggedIn;

        #endregion

        #region Properties

        public string Token
        {
            get
            {
                return ertisWebService.Token;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }

            private set
            {
                this.isLoggedIn = value;
            }
        }

        #endregion

        #region Constructors

        public AuthenticationService(IErtisWebService ertisWebService, ISessionManager sessionManager)
        {
            this.ertisWebService = ertisWebService;
            this.sessionManager = sessionManager;

            // Splash test;
            // UIHelper.Background(() => Thread.Sleep(30000));
        }

        #endregion

        #region Events

        public event EventHandler<int> OnLoginSuccess;
        public event EventHandler<string> OnLoginFailed;

        #endregion

        #region Methods

        public bool Login(string username, string password)
        {
            var loginResult = this.ertisWebService.Login(username, password);

            if (loginResult.IsSuccess)
            {
                this.IsLoggedIn = true;

                var tokenResponse = loginResult.Data as TokenResponse;
                int userID = tokenResponse.UserID;
                sessionManager.CreateSession(userID, tokenResponse.AccessToken);

                if (this.OnLoginSuccess != null)
                {
                    this.OnLoginSuccess(this, userID);
                }
            }
            else
            {
                this.IsLoggedIn = false;
                if (this.OnLoginFailed != null)
                {
                    this.OnLoginFailed(this, loginResult.Message);
                }
            }

            return this.IsLoggedIn;
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            return Task.Run(() => this.Login(username, password));
        }

        /*
        private Configuration GetConfiguration()
        {
            Configuration config = new Configuration(new ApiClient(basepath));
            config.DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.fffzzz";

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.fffzz'00'"
            };

            return config;
        }
        */

        #endregion
    }
}
