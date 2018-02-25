using Ertis.Client.Models;
using Ertis.Client.Services.Contracts;
using Ertis.Core.Connect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Ertis.Core.Profile;
using Ertis.Core.Human;
using Ertis.Core.Organization;

namespace Ertis.Client.Services
{
    public class ErtisWebService : IErtisWebService
    {
        #region Fields

        private string token;

        #endregion

        #region Properties

        public string Token
        {
            get
            {
                return token;
            }

            private set
            {
                this.token = value;
            }
        }

        #endregion

        #region Constructors

        public ErtisWebService()
        {

        }

        #endregion

        #region Request/Response Methods

        private ResponseResult GetRequest(string url, KeyValuePair<string, string>[] parameters, bool isAuth = true)
        {
            return this.SendRequest(url, parameters, new HttpMethod("GET"), isAuth);
        }

        private ResponseResult PostRequest(string url, KeyValuePair<string, string>[] parameters, bool isAuth = true)
        {
            return this.SendRequest(url, parameters, new HttpMethod("POST"), isAuth);
        }

        private ResponseResult PutRequest(string url, KeyValuePair<string, string>[] parameters, bool isAuth = true)
        {
            return this.SendRequest(url, parameters, new HttpMethod("PUT"), isAuth);
        }

        private ResponseResult DeleteRequest(string url, KeyValuePair<string, string>[] parameters, bool isAuth = true)
        {
            return this.SendRequest(url, parameters, new HttpMethod("DELETE"), isAuth);
        }

        private ResponseResult SendRequest(string url, KeyValuePair<string, string>[] parameters, HttpMethod method, bool isAuth)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = method.Method;

                if (method.Method == "POST")
                    request.ContentType = "application/x-www-form-urlencoded";

                if (method.Method == "GET" || method.Method == "PUT")
                    request.ContentType = "application/json";

                if (isAuth)
                    request.Headers = this.GetAuthenticatedHeaders();

                if (parameters != null)
                {
                    var data = this.ParamsToByteArray(parameters);
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }

                var response = (HttpWebResponse)request.GetResponse();
                string responseText = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new ResponseResult(true, responseText);
                }
                else
                {
                    return new ResponseResult(false, responseText);
                }
            }
            catch (System.Net.WebException webException)
            {
                if (webException.Message.Contains("401"))
                {
                    System.Diagnostics.Debug.WriteLine("UsernameOrPasswordIncorrect : " + webException.Message);
                    return new ResponseResult(false, "UsernameOrPasswordIncorrect");
                }

                if (webException.Message.Contains("423"))
                {
                    System.Diagnostics.Debug.WriteLine("UserDeactivated : " + webException.Message);
                    return new ResponseResult(false, "UserDeactivated");
                }

                System.Diagnostics.Debug.WriteLine("CouldNotConnectToServer : " + webException.Message);
                return new ResponseResult(false, "CouldNotConnectToServer");
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, "ErtisWebService.SendRequest error : " + ex.Message);
            }
        }

        private byte[] ParamsToByteArray(KeyValuePair<string, string>[] parameters)
        {
            string parametersString = string.Empty;
            foreach (var pair in parameters)
            {
                string param = string.Format("{0}={1}", pair.Key, pair.Value);
                parametersString += param + "&";
            }

            if (parametersString.Last() == '&')
                parametersString = parametersString.Substring(0, parametersString.Length - 1);

            return Encoding.ASCII.GetBytes(parametersString);
        }

        #endregion
        
        #region Login/Authentication Methods

        public ResponseResult Login(string username, string password)
        {
            var loginResult = this.TryLogin(username, password);

            if (loginResult.IsSuccess)
            {
                this.Token = loginResult.Data.ToString();

                // TODO
            }
            else
            {
                // TODO
            }

            return loginResult;
        }
        
        private ResponseResult TryLogin(string username, string password)
        {
            // 1. GetToken
            var tokenResult = this.GetAuthenticationToken(username, password);

            if (tokenResult.IsSuccess)
            {
                // TODO
            }
            else
            {
                // TODO
            }

            return tokenResult;
        }

        private ResponseResult GetAuthenticationToken(string username, string password)
        {
            KeyValuePair<string, string>[] parameters = new KeyValuePair<string, string>[2];
            parameters[0] = new KeyValuePair<string, string>("username", username);
            parameters[1] = new KeyValuePair<string, string>("password", password);

            var response = this.PostRequest(ConnectionScheme.Current.LoginURL, parameters, false);
            if (response.IsSuccess)
            {
                TokenResponse tokenResponse = TokenResponse.Create(response.Message);
                response.Data = tokenResponse;
            }

            return response;
        }
        
        private WebHeaderCollection GetAuthenticatedHeaders()
        {
            WebHeaderCollection headers = new WebHeaderCollection();

            headers.Add("Access-Control-Allow-Headers", "*");
            headers.Add("Access-Control-Allow-Origin", "*");
            headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", this.Token));

            return headers;
        }

        #endregion

        #region Company Methods

        public void GetCompany()
        {

        }

        #endregion

        #region User Methods

        public User GetUser(int userID)
        {
            var response = this.GetRequest(ConnectionScheme.Current.UserServiceURL + "/" + userID, null);

            if (response.IsSuccess)
            {
                User user = User.CreateFromJson(response.Message);
                return user;
            }

            return null;
        }

        public List<User> GetUserList()
        {
            var response = this.GetRequest(ConnectionScheme.Current.UserServiceURL, null);

            if (response.IsSuccess)
            {
                return User.ToListFromJson(response.Message);
            }

            return null;
        }

        public bool UpdateUser(User user)
        {
            var response = this.PutRequest(ConnectionScheme.Current.UserServiceURL + "/" + user.Id, null, true);
            return response.IsSuccess;
        }

        public bool DeleteUser(User user)
        {
            var response = this.DeleteRequest(ConnectionScheme.Current.UserServiceURL + "/" + user.Id, null, true);
            return response.IsSuccess;
        }

        public bool ActivateUser(User user)
        {
            var response = this.PutRequest(ConnectionScheme.Current.UserServiceURL + "/activate/" + user.Id, null, true);
            return response.IsSuccess;
        }

        public bool DeactivateUser(User user)
        {
            var response = this.PutRequest(ConnectionScheme.Current.UserServiceURL + "/deactivate/" + user.Id, null, true);
            return response.IsSuccess;
        }

        #endregion

        #region Staff Methods

        public Staff GetStaff(int staffID)
        {
            var response = this.GetRequest(ConnectionScheme.Current.StaffServiceURL + "/" + staffID, null);

            if (response.IsSuccess)
            {
                Staff staff = Ertis.Core.Serialization.SerializationHelper.DeserializeStaff(response.Message);
                return staff;
            }

            return null;
        }

        public List<Staff> GetStaffList()
        {
            var response = this.GetRequest(ConnectionScheme.Current.StaffServiceURL, null);

            if (response.IsSuccess)
            {
                return Ertis.Core.Serialization.SerializationHelper.DeserializeStaffList(response.Message);
            }

            return null;
        }

        public bool UpdateStaff(Staff staff)
        {
            var response = this.PutRequest(ConnectionScheme.Current.StaffServiceURL + "/" + staff.Id, null, true);
            return response.IsSuccess;
        }

        public bool DeleteStaff(Staff staff)
        {
            var response = this.DeleteRequest(ConnectionScheme.Current.StaffServiceURL + "/" + staff.Id, null, true);
            return response.IsSuccess;
        }

        #endregion

        #region Position Methods

        public Position GetPosition(int positionId)
        {
            var response = this.GetRequest(ConnectionScheme.Current.PositionServiceURL + "/" + positionId, null);

            if (response.IsSuccess)
            {
                Position position = Position.CreateFromJson(response.Message);
                return position;
            }

            return null;
        }

        public List<Position> GetPositionList()
        {
            var response = this.GetRequest(ConnectionScheme.Current.PositionServiceURL, null);

            if (response.IsSuccess)
            {
                return Position.ToListFromJson(response.Message);
            }

            return null;
        }

        #endregion

        #region Department Methods

        public Department GetDepartment(int departmentID)
        {
            var response = this.GetRequest(ConnectionScheme.Current.DepartmentServiceURL + "/" + departmentID, null);

            if (response.IsSuccess)
            {
                Department department = Ertis.Core.Serialization.SerializationHelper.DeserializeDepartment(response.Message);
                return department;
            }

            return null;
        }

        public List<Department> GetDepartmentList()
        {
            var response = this.GetRequest(ConnectionScheme.Current.DepartmentServiceURL, null);

            if (response.IsSuccess)
            {
                return Ertis.Core.Serialization.SerializationHelper.DeserializeDepartmentList(response.Message);
            }

            return null;
        }

        public bool UpdateDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Section Methods

        public Section GetSection(int sectionID)
        {
            var response = this.GetRequest(ConnectionScheme.Current.SectionServiceURL + "/" + sectionID, null);

            if (response.IsSuccess)
            {
                Section section = Ertis.Core.Serialization.SerializationHelper.DeserializeSection(response.Message);
                return section;
            }

            return null;
        }

        public List<Section> GetSectionList()
        {
            var response = this.GetRequest(ConnectionScheme.Current.SectionServiceURL, null);

            if (response.IsSuccess)
            {
                return Ertis.Core.Serialization.SerializationHelper.DeserializeSectionList(response.Message);
            }

            return null;
        }

        public bool UpdateSection(Section section)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSection(Section section)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Team Methods

        public Team GetTeam(int teamID)
        {
            var response = this.GetRequest(ConnectionScheme.Current.TeamServiceURL + "/" + teamID, null);

            if (response.IsSuccess)
            {
                Team team = Ertis.Core.Serialization.SerializationHelper.DeserializeTeam(response.Message);
                return team;
            }

            return null;
        }

        public List<Team> GetTeamList()
        {
            var response = this.GetRequest(ConnectionScheme.Current.TeamServiceURL, null);

            if (response.IsSuccess)
            {
                return Ertis.Core.Serialization.SerializationHelper.DeserializeTeamList(response.Message);
            }

            return null;
        }

        public bool UpdateTeam(Team team)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTeam(Team team)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
