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
    public class CredentialsRepository : RepositoryBase, ICredentialsRepository
    {
        private readonly string TABLE_NAME = "Credentials";

        /// <summary>
        /// Constructor
        /// </summary>
        public CredentialsRepository(IServiceProvider serviceProvider, IOptions<ConnectionString> dbConfig) : base(dbConfig.Value.ToString())
        {

        }

        public Credentials Add(string name, string surname, string emailAddress, string phoneNumber, DateTime? birthDate)
        {
            Credentials credentials = null;

            try
            {
                MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);

                string query = string.Format(
                    @"INSERT INTO {0} " +
                    "(Name, Surname, EmailAddress, PhoneNumber, BirthDate) " +
                    "VALUES(?name, ?surname, ?emailAddress, ?phoneNumber, ?birthDate);", TABLE_NAME);

                connection.Open();

                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = query
                };

                command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("?surname", MySqlDbType.VarChar).Value = surname;
                command.Parameters.Add("?emailAddress", MySqlDbType.VarChar).Value = emailAddress;
                command.Parameters.Add("?phoneNumber", MySqlDbType.VarChar).Value = phoneNumber;
                command.Parameters.Add("?birthDate", MySqlDbType.Date).Value = birthDate;

                int affectedRowCount = command.ExecuteNonQuery();

                if (affectedRowCount != 1)
                {
                    string exMessage = string.Format("CredentialsRepository.AddCredentials error! : AffectedRowCount = {0}", affectedRowCount);
                    System.Diagnostics.Debug.WriteLine(exMessage);
                    throw new Exception(exMessage);
                }
                
                credentials = this.Get((int)command.LastInsertedId, connection);

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

            return credentials;
        }

        public Credentials Get(int id)
        {
            Credentials credentials = null;

            try
            {
                MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);
                connection.Open();
                credentials = this.Get(id, connection);
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

            return credentials;
        }

        private Credentials Get(int id, MySqlConnection connection)
        {
            Credentials credentials = null;

            try
            {
                string query = string.Format("SELECT * FROM {0} WHERE id = {1};", TABLE_NAME, id);
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = $"{reader["Name"]}";
                        string surname = $"{reader["Surname"]}";
                        string emailAddress = $"{reader["EmailAddress"]}";
                        string phoneNumber = $"{reader["PhoneNumber"]}";
                        DateTime birthDate = DateTime.Parse($"{reader["BirthDate"]}");

                        credentials = new Credentials(id, name, surname, birthDate, emailAddress, phoneNumber);
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

            return credentials;
        }

        public List<Credentials> GetList()
        {
            List<Credentials> credentialsList = new List<Credentials>();

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
                            string name = $"{reader["Name"]}";
                            string surname = $"{reader["Surname"]}";
                            string emailAddress = $"{reader["EmailAddress"]}";
                            string phoneNumber = $"{reader["PhoneNumber"]}";
                            DateTime birthDate = DateTime.Parse($"{reader["BirthDate"]}");

                            Credentials credentials = new Credentials(id, name, surname, birthDate, emailAddress, phoneNumber);
                            credentialsList.Add(credentials);
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

            return credentialsList;
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

        public bool Remove(Credentials credentials)
        {
            if (credentials != null)
            {
                return this.Remove(credentials.Id);
            }

            return false;
        }

        public bool Update(int id, string name = null, string surname = null, DateTime? birthDate = null, string emailAddress = null, string phoneNumber = null)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING);

                List<string> queryPartList = new List<string>();

                string sqlQuery = @"UPDATE {0} SET ";

                if (!string.IsNullOrEmpty(name))
                    queryPartList.Add("Name=?name");
                if (!string.IsNullOrEmpty(surname))
                    queryPartList.Add("Surname=?surname");
                if (birthDate != null)
                    queryPartList.Add("BirthDate=?birthDate");
                if (!string.IsNullOrEmpty(emailAddress))
                    queryPartList.Add("EmailAddress=?emailAddress");
                if (!string.IsNullOrEmpty(phoneNumber))
                    queryPartList.Add("PhoneNumber=?phoneNumber");
                
                sqlQuery += string.Join(", ", queryPartList);
                sqlQuery += " WHERE Id=?id";

                string query = string.Format(sqlQuery, TABLE_NAME);

                connection.Open();

                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = query
                };

                // ID
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                // Name
                if (!string.IsNullOrEmpty(name))
                    command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;

                // Surname
                if (!string.IsNullOrEmpty(surname))
                    command.Parameters.Add("?surname", MySqlDbType.VarChar).Value = surname;

                // EmailAddress
                if (!string.IsNullOrEmpty(emailAddress))
                    command.Parameters.Add("?emailAddress", MySqlDbType.VarChar).Value = emailAddress;
                
                // PhoneNumber
                if (!string.IsNullOrEmpty(phoneNumber))
                    command.Parameters.Add("?phoneNumber", MySqlDbType.VarChar).Value = phoneNumber;

                // BirthDate
                if (birthDate != null)
                    command.Parameters.Add("?birthDate", MySqlDbType.Date).Value = birthDate.Value;

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
    }
}
