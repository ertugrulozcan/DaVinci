using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Ertis.WebService.Auth;
using Ertis.WebService.Dao;
using Ertis.WebService.Helpers;
using Ertis.WebService.Models;
using Ertis.WebService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ertis.Core.Profile;
using Ertis.Core.Data;
using Ertis.Core.Server;
using Ertis.WebService.Services;

namespace Ertis.WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowSpecificOrigin")]
    public class AccountController : Controller
    {
        #region Services

        public IUserService userService { get; }

        #endregion

        #region Fields

        private readonly JWTSettings jwtSettings;

        #endregion

        #region Constructors

        public AccountController(IOptions<JWTSettings> options)
        {
            this.userService = ServiceProvider.Current.UserService;
            this.jwtSettings = options.Value;
        }

        #endregion

        #region Methods

        [HttpPost]
        [ActionName("Register")]
        public HttpResponse<User> Register([FromBody]RegistrationForm form)
        {
            HttpResponse<User> httpResponse;

            try
            {
                bool isActive = true;
                if (form.IsActive != null)
                    isActive = form.IsActive.Value;

                UserRole role = UserRole.Unrole;
                if (form.UserRoleID != null)
                {
                    var userRoleList = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
                    if (userRoleList.Any(x => form.UserRoleID.Value == (int)x))
                    {
                        role = (UserRole)form.UserRoleID.Value;

                        // Admin ve SystemAdmin'e register esnasinda izin verilmez!
                        if (role == UserRole.Admin || role == UserRole.SystemAdmin)
                        {
                            httpResponse = new HttpResponse<User>(HttpStatusCode.BadRequest);
                            httpResponse.Message = "AdminAndSystemAdminCanNotSetOnRegister";
                            return httpResponse;
                        }
                    }
                }

                Result result = this.userService.Register(form.Card, form.Password, isActive, role);
                string resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                if (result.Success)
                {
                    httpResponse = new HttpResponse<User>(HttpStatusCode.OK);
                    httpResponse.Message = result.Message;
                    httpResponse.Data = result.Data as User;
                    return httpResponse;
                }
                else
                {
                    if (result.Error != null)
                    {
                        if (result.Error.ErrorCode == Errors.EMAIL_ALREADY_EXIST.ErrorCode)
                        {
                            httpResponse = new HttpResponse<User>(HttpStatusCode.Conflict);
                            httpResponse.Message = result.Message;
                            httpResponse.Error = resultJson;
                            return httpResponse;
                        }
                    }

                    httpResponse = new HttpResponse<User>(HttpStatusCode.BadRequest);
                    httpResponse.Message = result.Message;
                    httpResponse.Error = resultJson;
                    return httpResponse;
                }
            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponse<User>(HttpStatusCode.InternalServerError);
                httpResponse.Message = ex.Message;
                httpResponse.Error = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                return httpResponse;
            }
        }

        /*
        [HttpPost]
        [ActionName("Register")]
        public HttpResponseMessage Register([FromBody]RegistrationForm form)
        {
            HttpResponseMessage httpResponse;

            try
            {
                bool isActive = true;
                if (form.IsActive != null)
                    isActive = form.IsActive.Value;

                UserRoles role = UserRoles.Unrole;
                if (form.UserRoleID != null)
                {
                    var userRoleList = Enum.GetValues(typeof(UserRoles)).Cast<UserRoles>();
                    if (userRoleList.Any(x => form.UserRoleID.Value == (int)x))
                    {
                        role = (UserRoles)form.UserRoleID.Value;

                        // Admin ve SystemAdmin'e register esnasinda izin verilmez!
                        if (role == UserRoles.Admin || role == UserRoles.SystemAdmin)
                        {
                            httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            httpResponse.ReasonPhrase = "AdminAndSystemAdminCanNotSetOnRegister";
                            httpResponse.Content = new StringContent("Admin ve SystemAdmin'e register esnasinda izin verilmez!", System.Text.Encoding.UTF8, "application/json");
                            return httpResponse;
                        }
                    }
                }

                Result result = this.userService.Register(form.Card, form.Password, isActive, role);
                string resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                if (result.Success)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    httpResponse.Content = new StringContent(resultJson, System.Text.Encoding.UTF8, "application/json");

                    return httpResponse;
                }
                else
                {
                    if (result.Error != null)
                    {
                        if (result.Error.ErrorCode == Errors.EMAIL_ALREADY_EXIST.ErrorCode)
                        {
                            httpResponse = new HttpResponseMessage(HttpStatusCode.Conflict);
                            httpResponse.ReasonPhrase = result.Message;
                            httpResponse.Content = new StringContent(resultJson, System.Text.Encoding.UTF8, "application/json");
                            return httpResponse;
                        }
                    }

                    httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    httpResponse.ReasonPhrase = result.Message;
                    httpResponse.Content = new StringContent(resultJson, System.Text.Encoding.UTF8, "application/json");
                    return httpResponse;
                }
            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                httpResponse.ReasonPhrase = ex.Message;
                httpResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(ex), System.Text.Encoding.UTF8, "application/json");
                return httpResponse;
            }
        }
        */

        /*
        [HttpPost]
        [ActionName("Login")]
        public HttpResponseMessage Login([FromBody]LoginForm form)
        {
            // Token'a yonlendir!

            if (result.Success)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    // ResultMessage???
                };
            }
        }
        */

        #endregion
    }
}
