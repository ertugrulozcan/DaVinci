using System;
using System.Linq;
using System.Collections.Generic;
using Ertis.WebService.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MySql.Data.MySqlClient;
using Ertis.WebService.Dao.MySql;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Permissions;
using Ertis.Core.Profile;
using Ertis.WebService.Dao.Contracts;
using Ertis.Core.Human;

namespace Ertis.WebService.Dao.MySql
{
    public class UserRepository : RepositoryBase, IUserRepository
	{
		private readonly string TABLE_NAME = "Users";

        private ICredentialsRepository credentialsRepository;
        public ICredentialsRepository CredentialsRepository
        {
            get
            {
                if (this.credentialsRepository == null)
                    this.credentialsRepository = serviceProvider.GetService<ICredentialsRepository>();

                return this.credentialsRepository;
            }
        }

		/// <summary>
		/// Constructor
		/// </summary>
        public UserRepository(IServiceProvider serviceProvider, IOptions<ConnectionString> dbConfig) : base(dbConfig.Value.ToString())
		{
            this.serviceProvider = serviceProvider;
		}

		public User Add(UserCard userCard, string passwordHash)
		{
            return this.Add(userCard, passwordHash, true, UserRole.Unrole);
		}

        public User Add(UserCard userCard, string passwordHash, bool isActive, UserRole role)
        {
            User user = null;
            userCard.JoinDate = DateTime.Now;

            try
            {
                var credentials = this.CredentialsRepository.Add(userCard.Name, userCard.Surname, userCard.EmailAddress, userCard.PhoneNumber, userCard.BirthDate);

                MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);

                string query = string.Format(
                    @"INSERT INTO {0} " +
                    "(UserIdentifier, CredentialID, UserRoleID, JoinDate, PasswordHash, IsActive) " +
                    "VALUES(?userIdentifier, ?credentialID, ?userRoleId, ?joinDate, ?passwordHash, ?isActive);", TABLE_NAME);
                
                connection.Open();

                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = query
                };

                command.Parameters.Add("?userIdentifier", MySqlDbType.VarChar).Value = userCard.EmailAddress;
                command.Parameters.Add("?credentialID", MySqlDbType.Int32).Value = credentials.Id;
                command.Parameters.Add("?userRoleId", MySqlDbType.Int32).Value = (int)role;
                command.Parameters.Add("?joinDate", MySqlDbType.DateTime).Value = userCard.JoinDate;
                command.Parameters.Add("?passwordHash", MySqlDbType.VarChar).Value = passwordHash;
                command.Parameters.Add("?isActive", MySqlDbType.Bit).Value = isActive ? 1 : 0;

                command.ExecuteNonQuery();
                user = this.Get((int)command.LastInsertedId, credentials, connection);

                connection.Close();
            }
            catch (MySqlException mysqlEx)
            {
                System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                return null;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }

            return user;
        }

		public User Get(int id)
		{
			User user = null;

			try
			{
                MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);
                connection.Open();
                user = this.Get(id, null, connection);
				connection.Close();
			}
			catch (MySqlException mysqlEx)
			{
				System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
				return null;
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
				return null;
			}

			return user;
		}

        private User Get(int id, Credentials credentials, MySqlConnection connection)
		{
			User user = null;

			try
			{
				string query = string.Format("SELECT * FROM {0} WHERE Id = {1};", TABLE_NAME, id);
				MySqlCommand command = new MySqlCommand(query, connection);
				
				using (MySqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
                        string userIdentifier = $"{reader["UserIdentifier"]}";
                        Int32.TryParse($"{reader["CredentialID"]}", out int credentialID);
						
                        bool isParsed = Int32.TryParse($"{reader["UserRoleID"]}", out int roleValue);
                        UserRole userRole = UserRole.Unrole;
                        if (isParsed)
                            userRole = (UserRole)roleValue;

                        DateTime joinDate = DateTime.Parse($"{reader["JoinDate"]}");

                        bool isActive = true;
                        var isActiveStr = $"{reader["IsActive"]}";
                        if (!string.IsNullOrEmpty(isActiveStr))
                        {
                            isActiveStr = isActiveStr.ToUpper();
                            if (isActiveStr == "False".ToUpper() || isActiveStr == "0")
                                isActive = false;
                        }

                        if (credentials == null)
                            credentials = this.CredentialsRepository.Get(credentialID);

                        UserCard userCard = new UserCard(credentialID) { JoinDate = joinDate };
                        if (credentials != null)
                        {
                            userCard.Name = credentials.Name;
                            userCard.Surname = credentials.Surname;
                            userCard.EmailAddress = credentials.EmailAddress;
                            userCard.PhoneNumber = credentials.PhoneNumber;
                            userCard.BirthDate = credentials.BirthDate;
                        }
						
                        user = new User(id, userCard) { UserRole = userRole, IsActive = isActive };
					}
				}
			}
			catch (MySqlException mysqlEx)
			{
				System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
				return null;
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
				return null;
			}

			return user;
		}

        public string GetPasswordHash(int id)
		{
			string hash = null;

			try
			{
				MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);
				connection.Open();
				string query = string.Format("SELECT passwordHash FROM {0} WHERE Id = {1};", TABLE_NAME, id);
				MySqlCommand command = new MySqlCommand(query, connection);

				using (MySqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						hash = $"{reader["PasswordHash"]}";
					}
				}

                connection.Close();
			}
			catch (MySqlException mysqlEx)
			{
				System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
				return null;
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
				return null;
			}

			return hash;
		}

		public List<User> GetList()
		{
			List<User> userList = new List<User>();

			try
            {
				MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);
				connection.Open();

                MySqlCommand command = new MySqlCommand(string.Format("SELECT * FROM {0};", TABLE_NAME), connection);
				using (MySqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
                        try
                        {
                            int id = Convert.ToInt32($"{reader["Id"]}");
                            string userIdentifier = $"{reader["UserIdentifier"]}";
                            Int32.TryParse($"{reader["CredentialID"]}", out int credentialID);

                            bool isParsed = Int32.TryParse($"{reader["UserRoleID"]}", out int roleValue);
                            UserRole userRole = UserRole.Unrole;
                            if (isParsed)
                                userRole = (UserRole)roleValue;

                            DateTime joinDate = DateTime.Parse($"{reader["JoinDate"]}");

                            bool isActive = true;
                            var isActiveStr = $"{reader["IsActive"]}";
                            if (!string.IsNullOrEmpty(isActiveStr))
                            {
                                isActiveStr = isActiveStr.ToUpper();
                                if (isActiveStr == "False".ToUpper() || isActiveStr == "0")
                                    isActive = false;
                            }

                            UserCard userCard = new UserCard(credentialID)
                            {
                                JoinDate = joinDate,
                            };

                            User user = new User(id, userCard) { UserRole = userRole, IsActive = isActive };
                            userList.Add(user);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }
					}
				}

				connection.Close();
            }
			catch (MySqlException mysqlEx)
			{
				System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
				return null;
			}
            catch (System.Exception ex)
            {
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
				return null;
			}

			return userList;
		}

        public bool Remove(int id)
		{
			try
			{
				MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);

				string query = string.Format(
					@"DELETE FROM {0} " +
					"WHERE Id=?id", TABLE_NAME);

				connection.Open();

                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = query
                };

                command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
				command.ExecuteNonQuery();
				connection.Close();

                return true;
			}
			catch (MySqlException mysqlEx)
			{
				System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                return false;
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return false;
			}
		}

        public bool Remove(User user)
		{
			if (user != null)
			{
				return this.Remove(user.Id);
			}

            return false;
		}

        public bool Update(int id, string name = null, string surname = null, DateTime? birthDate = null, string phoneNumber = null, bool? isActive = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(surname) || birthDate != null || !string.IsNullOrEmpty(phoneNumber))
                {
                    User user = this.Get(id);    
                    bool isSuccess = this.CredentialsRepository.Update(user.Card.Id, name, surname, birthDate, null, phoneNumber);
                }

                if (isActive != null)
                {
                    MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);

                    List<string> queryPartList = new List<string>();

                    string sqlQuery = @"UPDATE {0} SET ";

                    if (isActive != null)
                        queryPartList.Add("IsActive=?isActive");

                    sqlQuery += string.Join(", ", queryPartList);
                    sqlQuery += " WHERE Id=?id";

                    string query = string.Format(sqlQuery, TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    // UserID
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                    // IsActive
                    if (isActive != null)
                        command.Parameters.Add("?isActive", MySqlDbType.Bit).Value = isActive.Value ? 1 : 0;

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return true;
            }
            catch (MySqlException mysqlEx)
            {
                System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                return false;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return false;
            }
        }

		public bool Update(int id, UserCard userCard)
		{
            if (userCard == null)
            {
                System.Diagnostics.Debug.WriteLine("Update failed, UserCard is null!");
                return false;
            }

            return this.Update(id, userCard.Name, userCard.Surname, userCard.BirthDate, userCard.PhoneNumber);
		}
	}
}