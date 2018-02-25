using System;
using System.Collections.Generic;
using Ertis.Core.Human;
using Ertis.Core.Organization;
using Ertis.Core.Profile;
using Ertis.WebService.Dao.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Ertis.WebService.Models;

namespace Ertis.WebService.Dao.MySql
{
    public class StaffRepository : RepositoryBase, IStaffRepository
    {
        #region Constants

        private readonly string STAFFS_TABLE_NAME = "Staffs";
        private readonly string POSITIONS_TABLE_NAME = "Positions";

        #endregion

        #region Services

        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                    this.userRepository = serviceProvider.GetService<IUserRepository>();

                return this.userRepository;
            }
        }

        private IOrganizationRepository organizationRepository;
        public IOrganizationRepository OrganizationRepository
        {
            get
            {
                if (this.organizationRepository == null)
                    this.organizationRepository = serviceProvider.GetService<IOrganizationRepository>();

                return this.organizationRepository;
            }
        }

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

        #endregion

        #region Constructors

        public StaffRepository(IServiceProvider serviceProvider, IOptions<ConnectionString> dbConfig) : base(dbConfig.Value.ToString())
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Staff Methods

        public Staff GetStaff(int id)
        {
            Staff staff = null;

            try
            {
                string query = string.Format("SELECT * FROM {0} WHERE Id = {1};", STAFFS_TABLE_NAME, id);

                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            staff = this.ExtractStaffFromDataReader(id, reader);
                        }
                    }

                    connection.Close();
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

            return staff;
        }

        public List<Staff> GetStaffList()
        {
            List<Staff> staffList = new List<Staff>();

            try
            {
                string query = string.Format("SELECT * FROM {0};", STAFFS_TABLE_NAME);
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32($"{reader["Id"]}");
                            Staff staff = this.ExtractStaffFromDataReader(id, reader);
                            staffList.Add(staff);
                        }
                    }

                    connection.Close();
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

            return staffList;
        }

        private Staff ExtractStaffFromDataReader(int id, MySqlDataReader reader)
        {
            Int32.TryParse($"{reader["CredentialID"]}", out int credentialId);
            Int32.TryParse($"{reader["UserID"]}", out int userId);
            Int32.TryParse($"{reader["DepartmentID"]}", out int departmentId);
            Int32.TryParse($"{reader["SectionID"]}", out int sectionId);
            Int32.TryParse($"{reader["TeamID"]}", out int teamId);
            Int32.TryParse($"{reader["PositionID"]}", out int positionId);

            Credentials credentials = null;
            User user = null;
            Position position = null;
            Department department = null;
            Section section = null;
            Team team = null;

            if (this.IsRecursiveSelectionEnable)
            {
                credentials = this.CredentialsRepository.Get(credentialId);
                user = this.UserRepository.Get(userId);
                position = this.GetPosition(positionId);
                department = this.OrganizationRepository.GetDepartment(departmentId);
                section = this.OrganizationRepository.GetSection(sectionId);
                team = this.OrganizationRepository.GetTeam(teamId);

                return new Staff(id, user, position)
                {
                    UserCredentials = credentials,
                    Department = department,
                    Section = section,
                    Team = team
                };
            }
            else
            {
                return new Staff(id, credentialId, userId, positionId, departmentId, sectionId, teamId);
            }
        }

        public Staff AddStaff(StaffForm form)
        {
            Staff staff = null;
            Credentials credentials = null;

            try
            {
                credentials = this.CredentialsRepository.Add(form.Name, form.Surname, form.EmailAddress, form.PhoneNumber, form.BirthDate);

                if (credentials == null)
                {
                    string exMessage = string.Format("StaffRepository.AddStaff error! : Credentials could not created.");
                    System.Diagnostics.Debug.WriteLine(exMessage);
                    throw new Exception(exMessage);
                }

                List<string> queryParamList = new List<string>();
                List<string> queryValueList = new List<string>();

                if (credentials != null)
                {
                    queryParamList.Add("CredentialID");
                    queryValueList.Add("?credentialId");
                }

                if (form.UserID != null)
                {
                    queryParamList.Add("UserID");
                    queryValueList.Add("?userId");
                }

                if (form.DepartmentID != null)
                {
                    queryParamList.Add("DepartmentID");
                    queryValueList.Add("?departmentId");
                }

                if (form.SectionID != null)
                {
                    queryParamList.Add("SectionID");
                    queryValueList.Add("?sectionId");
                }

                if (form.TeamID != null)
                {
                    queryParamList.Add("TeamID");
                    queryValueList.Add("?teamId");
                }

                if (form.PositionID != null)
                {
                    queryParamList.Add("PositionID");
                    queryValueList.Add("?positionId");
                }

                string queryParamsString = string.Join(", ", queryParamList);
                string queryValueString = string.Join(", ", queryValueList);

                string query = string.Format(@"INSERT INTO {0} ({1}) VALUES({2});", STAFFS_TABLE_NAME, queryParamsString, queryValueString);

                MySqlCommand command = new MySqlCommand();
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    if (credentials != null)
                        command.Parameters.Add("?credentialId", MySqlDbType.Int32).Value = credentials.Id;

                    if (form.UserID != null)
                        command.Parameters.Add("?userId", MySqlDbType.Int32).Value = form.UserID;

                    if (form.DepartmentID != null)
                        command.Parameters.Add("?departmentId", MySqlDbType.Int32).Value = form.DepartmentID;

                    if (form.SectionID != null)
                        command.Parameters.Add("?sectionId", MySqlDbType.Int32).Value = form.SectionID;

                    if (form.TeamID != null)
                        command.Parameters.Add("?teamId", MySqlDbType.Int32).Value = form.TeamID;

                    if (form.PositionID != null)
                        command.Parameters.Add("?positionId", MySqlDbType.Int32).Value = form.PositionID;

                    command.ExecuteNonQuery();
                    staff = this.GetStaff((int)command.LastInsertedId);
                    staff.UserCredentials = credentials;

                    connection.Close();
                }
            }
            catch (MySqlException mysqlEx)
            {
                System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                // RollBack for credentials
                if (staff == null && credentials != null)
                    this.credentialsRepository.Remove(credentials);
            }

            return staff;
        }

        public bool RemoveStaff(int id)
        {
            try
            {
                string query = string.Format(@"DELETE FROM {0} " + "WHERE Id=?id", STAFFS_TABLE_NAME);

                MySqlCommand command = new MySqlCommand();
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
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

        public bool UpdateStaff(int id, StaffForm form)
        {
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                connection.Open();
                bool result = this.UpdateStaff(
                    id,
                    form.CredentialID,
                    form.Name,
                    form.Surname,
                    form.EmailAddress,
                    form.PhoneNumber,
                    form.BirthDate,
                    form.DepartmentID,
                    form.SectionID,
                    form.TeamID,
                    form.PositionID,
                    connection);
                connection.Close();

                return result;
            }
        }

        public bool UpdateStaff(
            int id,
            int? credentialId,
            string name = null,
            string surname = null,
            string emailAddress = null,
            string phoneNumber = null,
            DateTime? birthDate = null,
            int? departmentId = null,
            int? sectionId = null,
            int? teamId = null,
            int? positionId = null,
            MySqlConnection connection = null)
        {
            bool isSuccess = true;
                
            try
            {
                // Credentials Update
                if (credentialId != null)
                {
                    bool isCredentialsUpdated = this.credentialsRepository.Update(credentialId.Value, name, surname, birthDate, emailAddress, phoneNumber);
                    isSuccess &= isCredentialsUpdated;
                }

                // Others Update
                List<string> queryPartList = new List<string>();

                string sqlQuery = @"UPDATE {0} SET ";

                if (departmentId != null)
                    queryPartList.Add("DepartmentID=?departmentId");
                if (sectionId != null)
                    queryPartList.Add("SectionID=?sectionId");
                if (teamId != null)
                    queryPartList.Add("TeamID=?teamId");
                if (positionId != null)
                    queryPartList.Add("PositionID=?positionId");
                
                sqlQuery += string.Join(", ", queryPartList);
                sqlQuery += " WHERE Id=?id";

                MySqlCommand command = new MySqlCommand();
                string query = string.Format(sqlQuery, STAFFS_TABLE_NAME);

                if (connection == null)
                {
                    using (connection = new MySqlConnection(this.CONNECTION_STRING))
                    {
                        command.Connection = connection;
                        command.CommandText = query;

                        connection.Open();
                        isSuccess &= this.ExecuteUpdateStaffCommand(command, id, departmentId, sectionId, teamId, positionId);
                        connection.Close();
                    }
                }
                else
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    isSuccess &= this.ExecuteUpdateStaffCommand(command, id, departmentId, sectionId, teamId, positionId);
                }
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

            return isSuccess;
        }

        private bool ExecuteUpdateStaffCommand(
            MySqlCommand command,
            int id,
            int? departmentId = null,
            int? sectionId = null,
            int? teamId = null,
            int? positionId = null)
        {
            try
            {
                // UserID
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                // DepartmentId
                if (departmentId != null)
                    command.Parameters.Add("?departmentId", MySqlDbType.Int32).Value = departmentId;

                // SectionId
                if (sectionId != null)
                    command.Parameters.Add("?sectionId", MySqlDbType.Int32).Value = sectionId;

                // TeamId
                if (teamId != null)
                    command.Parameters.Add("?teamId", MySqlDbType.Int32).Value = teamId;

                // PositionId
                if (positionId != null)
                    command.Parameters.Add("?positionId", MySqlDbType.Int32).Value = positionId;

                int affectedRowCount = command.ExecuteNonQuery();

                return affectedRowCount == 1;
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("StaffRepository.ExecuteUpdateStaffCommand() error! : {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine(exMessage);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                return false;
            }
        }

        #endregion

        #region Position Methods

        public Position GetPosition(int id)
        {
            Position position = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    connection.Open();

                    string query = string.Format("SELECT * FROM {0} WHERE Id = {1};", POSITIONS_TABLE_NAME, id);
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = $"{reader["Name"]}";
                            string description = $"{reader["Description"]}";

                            position = new Position(id, name) { Description = description };
                        }
                    }

                    connection.Close();
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

            return position;
        }

        public List<Position> GetPositionList()
        {
            List<Position> positionList = new List<Position>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(string.Format("SELECT * FROM {0};", POSITIONS_TABLE_NAME), connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                int id = Convert.ToInt32($"{reader["Id"]}");
                                string name = $"{reader["Name"]}";
                                string description = $"{reader["Description"]}";

                                Position position = new Position(id, name) { Description = description };
                                positionList.Add(position);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                            }
                        }
                    }

                    connection.Close();
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

            return positionList;
        }

        public Position AddPosition(PositionForm form)
        {
            Position position = null;

            try
            {
                List<string> queryParamList = new List<string>();
                List<string> queryValueList = new List<string>();

                if (form.ID != null)
                {
                    queryParamList.Add("Id");
                    queryValueList.Add("?id");
                }

                if (form.Name != null)
                {
                    queryParamList.Add("Name");
                    queryValueList.Add("?name");
                }

                if (form.Description != null)
                {
                    queryParamList.Add("Description");
                    queryValueList.Add("?description");
                }

                string queryParamsString = string.Join(", ", queryParamList);
                string queryValueString = string.Join(", ", queryValueList);

                string query = string.Format(@"INSERT INTO {0} ({1}) VALUES({2});", POSITIONS_TABLE_NAME, queryParamsString, queryValueString);

                MySqlCommand command = new MySqlCommand();
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    if (form.ID != null)
                        command.Parameters.Add("?id", MySqlDbType.Int32).Value = form.ID;

                    if (!string.IsNullOrEmpty(form.Name))
                        command.Parameters.Add("?name", MySqlDbType.VarChar).Value = form.Name;

                    if (!string.IsNullOrEmpty(form.Description))
                        command.Parameters.Add("?description", MySqlDbType.VarChar).Value = form.Description;

                    command.ExecuteNonQuery();
                    position = this.GetPosition((int)command.LastInsertedId);

                    connection.Close();
                }
            }
            catch (MySqlException mysqlEx)
            {
                System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return position;
        }

        public bool UpdatePosition(int id, PositionForm form)
        {
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                connection.Open();
                bool result = this.UpdatePosition(id, form, connection);
                connection.Close();

                return result;
            }
        }

        public bool UpdatePosition(int id, PositionForm form, MySqlConnection connection)
        {
            bool isSuccess = true;

            try
            {
                List<string> queryPartList = new List<string>();

                string sqlQuery = @"UPDATE {0} SET ";

                if (form.ID != null)
                    queryPartList.Add("Id=?id");
                if (!string.IsNullOrEmpty(form.Name))
                    queryPartList.Add("Name=?name");
                if (!string.IsNullOrEmpty(form.Description))
                    queryPartList.Add("Description=?description");
                
                sqlQuery += string.Join(", ", queryPartList);
                sqlQuery += " WHERE Id=?id";

                MySqlCommand command = new MySqlCommand();
                string query = string.Format(sqlQuery, POSITIONS_TABLE_NAME);

                if (connection == null)
                {
                    using (connection = new MySqlConnection(this.CONNECTION_STRING))
                    {
                        command.Connection = connection;
                        command.CommandText = query;

                        connection.Open();
                        isSuccess &= this.ExecuteUpdatePositionCommand(command, id, form.Name, form.Description);
                        connection.Close();
                    }
                }
                else
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    isSuccess &= this.ExecuteUpdatePositionCommand(command, id, form.Name, form.Description);
                }
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

            return isSuccess;
        }

        private bool ExecuteUpdatePositionCommand(MySqlCommand command, int id, string name, string description)
        {
            try
            {
                // ID
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                // Name
                if (!string.IsNullOrEmpty(name))
                    command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;

                // Description
                if (!string.IsNullOrEmpty(description))
                    command.Parameters.Add("?description", MySqlDbType.VarChar).Value = description;

                int affectedRowCount = command.ExecuteNonQuery();

                return affectedRowCount == 1;
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("StaffRepository.ExecuteUpdatePositionCommand() error! : {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine(exMessage);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public bool RemovePosition(int id)
        {
            try
            {
                string query = string.Format(@"DELETE FROM {0} " + "WHERE Id=?id", POSITIONS_TABLE_NAME);

                MySqlCommand command = new MySqlCommand();
                using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
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

        #endregion
    }
}
