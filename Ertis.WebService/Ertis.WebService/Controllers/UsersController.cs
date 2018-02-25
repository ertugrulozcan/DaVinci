using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Ertis.WebService.Auth;
using Ertis.WebService.Dao;
using Ertis.WebService.Models;
using Ertis.WebService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ertis.Core.Profile;
using Ertis.Core.Data;

namespace Ertis.WebService.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        #region Services

        public IUserService userService { get; }

        #endregion

        #region Constructors

        public UsersController()
        {
            this.userService = Services.ServiceProvider.Current.UserService;
        }

        #endregion

        #region Controller Methods

        // GET api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return this.userService.GetUserList();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return this.userService.GetUser(id);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public User Put(int id, [FromBody]UserCard user)
        {
            var result = this.userService.Update(id, user);
            if (result.Success)
                return result.Data as User;
            else
                return null;
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.userService.Remove(id);
        }

        [Route("activate/{id}")]
        public Result ActivateUser(int id)
        {
            return this.SetIsActive(id, true);
        }

        [Route("deactivate/{id}")]
        public Result DeactivateUser(int id)
        {
            return this.SetIsActive(id, false);
        }

        private Result SetIsActive(int id, bool isActive)
        {
            try
            {
                var requestOwner = this.GetRequestedUser();
                if (requestOwner.UserRole != UserRole.Admin && requestOwner.UserRole != UserRole.SystemAdmin)
                {
                    return new Result(false, 401, "YouAreNotAuthorizedToPerformThisOperation");
                }

                var user = this.userService.GetUser(id);
                if (user == null)
                {
                    return new Result(false, 400, "UserNotFound");
                }

                var result = this.userService.SetIsActive(id, isActive);

                if (result.Success)
                {
                    user.IsActive = isActive;
                    return new Result(true, 200, "SetIsActive successful.", user);
                }
                else
                {
                    return new Result(false, 500, result.Message);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, 500, ex.Message);
            }
        }

        #endregion

        #region Methods

        private User GetRequestedUser()
        {
            string userIdentifier = this.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            User user = this.userService.GetUser(userIdentifier);
            return user;
        }

        #endregion
    }
}
