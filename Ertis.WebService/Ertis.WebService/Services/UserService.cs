using System.Collections.Generic;
using Ertis.WebService.Dao.Contracts;
using Ertis.WebService.Models;
using Ertis.WebService.Services.Contracts;
using System.Linq;
using System;
using Ertis.WebService.Helpers;
using Ertis.Core.Profile;
using Ertis.Core.Server;
using Ertis.Core.Data;
using Ertis.Core.Human;

namespace Ertis.WebService.Services
{
	public class UserService : IUserService
	{
        #region Services

        private readonly IUserRepository userRepository;
        private readonly ICredentialsRepository credentialsRepository;
        private ISessionService sessionService;
        private IOrganizationService organizationService;

        #endregion

        #region Fields

        private List<User> userList;
        private List<Credentials> credentialsList;

		#endregion

		#region Properties

        public IOrganizationService OrganizationService
        {
            get
            {
                if (this.organizationService == null)
                    this.organizationService = ServiceProvider.Current.OrganizationService;

                return this.organizationService;
            }
        }

        public ISessionService SessionService
        {
            get
            {
                if (this.sessionService == null)
                    this.sessionService = ServiceProvider.Current.SessionService;

                return this.sessionService;
            }
        }

        public List<Credentials> CredentialsList
        {
            get
            {
                return this.credentialsList;
			}

            private set
            {
                this.credentialsList = value;
            }
        }

        public List<User> UserList
        {
            get
            {
                return this.userList;
            }

            private set
            {
                this.userList = value;
            }
        }

		#endregion

		#region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ertis.WebService.Services.UserService"/> class.
        /// </summary>
        /// <param name="userRepository">User repository.</param>
        /// <param name="sessionService">Session service.</param>
        public UserService(IUserRepository userRepository, ICredentialsRepository credentialsRepository)
		{
			this.userRepository = userRepository;
            this.credentialsRepository = credentialsRepository;

            this.UserList = this.userRepository.GetList();
            this.SetCredentials();
		}

		#endregion

		#region Methods

        private void SetCredentials()
        {
            if (this.UserList == null)
                return;

            this.CredentialsList = this.credentialsRepository.GetList();
            foreach (var user in this.UserList)
            {
                if (this.CredentialsList.Any(x => x.Id == user.Card.Id))
                {
                    var credentials = this.CredentialsList.First(x => x.Id == user.Card.Id);;
                    user.Card.Name = credentials.Name;
                    user.Card.Surname = credentials.Surname;
                    user.Card.EmailAddress = credentials.EmailAddress;
                    user.Card.PhoneNumber = credentials.PhoneNumber;
                    user.Card.BirthDate = credentials.BirthDate;
                }
            }
        }
            
        public Result Register(UserCard usercard, string password, bool isActive, UserRole userRole)
        {
            try
            {
                if (usercard == null || !usercard.IsValid())
                    return new Result(Errors.USERCARD_INVALID, 406);

                if (this.UserList.Any(x => x.Card.EmailAddress.ToLower() == usercard.EmailAddress.ToLower()))
                    return new Result(Errors.EMAIL_ALREADY_EXIST, 409);

                string passwordHash = Helpers.PasswordHasher.SHA2(password);
                User user = this.userRepository.Add(usercard, passwordHash, isActive, userRole);

                if (user == null)
                    return new Result(Errors.UNKNOWN_ERROR, 400);

                this.UserList.Add(user);

                var result = new Result(true, 200, "Register success!");
                result.Data = user;                
                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Register failed!");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                return new Result(false, 500, ex.Message) { Error = Errors.CreateUncategorizedError(ex.StackTrace.ToString()) };
            }
        }

        public List<User> GetUserList()
		{
			return this.UserList;
		}

        public User GetUser(int id)
        {
            return this.UserList.FirstOrDefault(x => x.Id == id);
        }

        public User GetUser(string userIdintifier)
        {
            if (string.IsNullOrEmpty(userIdintifier))
                return null;
            
            if (this.UserList.Any(x => x.UserIdintifier == userIdintifier))
                return this.UserList.First(x => x.UserIdintifier == userIdintifier);
            else
                return null;
        }

        public Result Update(int id, UserCard userCard)
        {
            try
            {
                User user = this.GetUser(id);
                if (user == null)
                    return new Result(false, 404, "User not found!");

                bool isSuccess = this.userRepository.Update(id, userCard);
                if (isSuccess)
                {
                    user.Card.Override(userCard);

                    return new Result(isSuccess, 200, "Update success!") { Data = user };
                }
                else
                    return new Result(isSuccess, 500, "Update failed!");
            }
            catch (Exception ex)
            {
				System.Diagnostics.Debug.WriteLine("Update failed!");
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				return new Result(false, 500, ex.Message);
            }
        }

        public Result Remove(int id)
        {
			try
			{
				User user = this.GetUser(id);
				if (user == null)
					return new Result(false, 404, "User not found!");

                bool isSuccess = this.userRepository.Remove(id);
                if (isSuccess)
                {
                    this.UserList.Remove(user);
                    var staffList = this.OrganizationService.GetStaffList();
                    if (!staffList.Any(x => x.CredentialsId == user.Card.Id))
                    {
                        this.RemoveCredentials(user.Card.Id);
                    }

                    return new Result(true, 200, "Remove success!");
                }
                else
                {
                    return new Result(true, 500, "Delete failed!");
                }
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Remove failed!");
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				return new Result(false, 500, ex.Message);
			}
        }

        public bool RemoveCredentials(int id)
        {
            bool isSuccess = this.credentialsRepository.Remove(id);
            if (isSuccess)
            {
                var removeCredentials = this.CredentialsList.FirstOrDefault(x => x.Id == id);
                this.CredentialsList.Remove(removeCredentials);
            }

            return isSuccess;
        }

        public string GetPasswordHash(User user)
        {
            return this.userRepository.GetPasswordHash(user.Id);
        }

		public bool ValidatePassword(User user, string password)
		{
			string hash1 = this.GetPasswordHash(user);
			string hash2 = Helpers.PasswordHasher.SHA2(password);

			return hash1 == hash2;
		}

        public Result SetIsActive(int userId, bool isActive)
        {
            if (this.SessionService != null)
            {
                bool isSuccess = this.userRepository.Update(userId, isActive:isActive);

                if (isSuccess)
                    return new Result(true, 200, data:this.GetUser(userId));
                else
                    return new Result(false, 500, "SetIsActive failed!");
            }
            else
            {
                return new Result(false, 500, "UnknownError");
            }
        }

        #endregion
    }
}