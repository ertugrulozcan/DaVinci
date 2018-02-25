using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Ertis.WebService.Services.Contracts;
using System.Linq;
using Ertis.WebService.Models;
using Ertis.Core.Data;
using Ertis.Core.Profile;
using Ertis.Core.Server;

namespace Ertis.WebService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Services

        private readonly IUserService userService;
        private readonly ISessionService sessionService;

        #endregion

        #region Constructors

        public AuthenticationService()
        {
            this.userService = ServiceProvider.Current.UserService;
            this.sessionService = ServiceProvider.Current.SessionService;
        }

        #endregion

        #region Methods

        public Result Login(string username, string password, string token, string appID)
        {
            try
            {
                var userList = this.userService.GetUserList();
                if (userList.Any(x => x.Card.EmailAddress.ToLower() == username.ToLower()))
                {
                    User user = userList.First(x => x.Card.EmailAddress.ToLower() == username.ToLower());
                    if (user.IsActive)
                    {
                        bool isPasswordCorrect = this.userService.ValidatePassword(user, password);

                        if (isPasswordCorrect)
                        {
                            Session session = this.sessionService.CreateSession(user, token, appID);

                            if (session != null)
                            {
                                var result = new Result(true, 200, "Login success!");
                                result.Data = user.Id;
                                return result;
                            }
                            else
                            {
                                return new Result(false, 500, "Login failed! (Internal Server Error)");
                            }
                        }
                        else
                        {
                            return new Result(false, 401, "Password invalid!");
                        }
                    }
                    else
                    {
                        return new Result(false, 423, "User account is passive!");
                    }
                }
                else
                {
                    return new Result(false, 401, "Username invalid!");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Login failed!");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                return new Result(false, 500, ex.Message);
            }
        }

        public Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var userList = this.userService.GetUserList();
            if (userList.Any(x => x.Card != null && x.Card.EmailAddress == username))
            {
                var user = userList.First(x => x.Card.EmailAddress == username);
                bool passwordIsCorrect = this.userService.ValidatePassword(user, password);

                if (passwordIsCorrect)
                {
                    var genericID = new GenericIdentity(username, "Token");
                    var result = Task.FromResult(new ClaimsIdentity(genericID, new Claim[] { }));
                    return result;
                }

                return Task.FromResult<ClaimsIdentity>(null);
            }

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>(null);
        }

        #endregion
    }
}