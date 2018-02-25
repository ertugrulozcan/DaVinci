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
using Ertis.Core.Organization;
using Ertis.Core.Human;

namespace Ertis.WebService.Dao.MySql
{
    public class OrganizationRepository : RepositoryBase, IOrganizationRepository
    {
        #region Constants

        private readonly string COMPANY_TABLE_NAME = "Company";
        private readonly string DEPARTMENTS_TABLE_NAME = "Departments";
        private readonly string SECTIONS_TABLE_NAME = "Sections";
        private readonly string TEAMS_TABLE_NAME = "Teams";

        #endregion

        #region Services

        private IStaffRepository staffRepository;
        public IStaffRepository StaffRepository
        {
            get
            {
                if (this.staffRepository == null)
                    this.staffRepository = serviceProvider.GetService<IStaffRepository>();

                return this.staffRepository;
            }
        }

        #endregion

        #region Constructors

        public OrganizationRepository(IServiceProvider serviceProvider, IOptions<ConnectionString> dbConfig) : base(dbConfig.Value.ToString())
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Company Methods

        public Company GetCompany()
        {
            Company company = null;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    string query = string.Format("SELECT * FROM {0};", COMPANY_TABLE_NAME);
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = Convert.ToInt32($"{reader["Id"]}");
                            string name = $"{reader["Name"]}";
                            string shortName = $"{reader["ShortName"]}";

                            company = new Company(id, name, shortName);
                        }
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
                    connection.Close();
                }
            }

            return company;
        }

        #endregion

        #region Department Methods

        public Department GetDepartment(int id)
        {
            Department department = null;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    string query = string.Format("SELECT * FROM {0} WHERE Id = {1};", DEPARTMENTS_TABLE_NAME, id);
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = $"{reader["Name"]}";
                            int managerId = Convert.ToInt32($"{reader["ManagerID"]}");

                            department = new Department(id, name, managerId);
                        }
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
                    connection.Close();
                }
            }

            return department;
        }

        public List<Department> GetDepartmentList()
        {
            List<Department> departmentList = new List<Department>();
            bool isSuccess = true;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(string.Format("SELECT * FROM {0};", DEPARTMENTS_TABLE_NAME), connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32($"{reader["Id"]}");
                            string name = $"{reader["Name"]}";
                            int managerId = Convert.ToInt32($"{reader["ManagerID"]}");

                            Department department = new Department(id, name, managerId);

                            departmentList.Add(department);
                            isSuccess &= true;
                        }
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return departmentList;
            else
                return null;
        }

        public Department AddDepartment(Department department)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"INSERT INTO {0} " + "(Name, ManagerID) " + "VALUES(?name, ?managerId);", DEPARTMENTS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?name", MySqlDbType.VarChar).Value = department.Name;
                    command.Parameters.Add("?managerId", MySqlDbType.Int32).Value = department.ManagerId;
                    command.ExecuteNonQuery();

                    department = new Department((int)command.LastInsertedId, department.Name, department.ManagerId);

                    isSuccess &= true;
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return department;
            else
                return null;
        }

        public Department AddDepartment(string name, int managerId)
        {
            Department department = null;
            bool isSuccess = true;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"INSERT INTO {0} " + "(Name, ManagerID) " + "VALUES(?name, ?managerId);", DEPARTMENTS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("?managerId", MySqlDbType.Int32).Value = managerId;
                    command.ExecuteNonQuery();

                    Staff manager = null;
                    if (this.IsRecursiveSelectionEnable)
                        manager = this.StaffRepository.GetStaff(managerId);
                    
                    department = new Department((int)command.LastInsertedId, name, managerId);

                    isSuccess &= true;
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return department;
            else
                return null;
        }

        public bool RemoveDepartment(int id)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"DELETE FROM {0} " + "WHERE Id=?id", DEPARTMENTS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                    command.ExecuteNonQuery();
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return isSuccess;
        }

        public bool UpdateDepartment(int id, DepartmentForm form)
        {
            return this.UpdateDepartment(id, form.Name, form.ManagerId);
        }

        public bool UpdateDepartment(int id, string name = null, int? managerId = null)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    List<string> queryPartList = new List<string>();

                    string sqlQuery = @"UPDATE {0} SET ";

                    if (!string.IsNullOrEmpty(name))
                        queryPartList.Add("Name=?name");
                    if (managerId != null)
                        queryPartList.Add("ManagerID=?managerId");

                    sqlQuery += string.Join(", ", queryPartList);
                    sqlQuery += " WHERE Id=?id";

                    string query = string.Format(sqlQuery, DEPARTMENTS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    // UserID
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                    // Name
                    if (!string.IsNullOrEmpty(name))
                        command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;

                    // ManagerId
                    if (managerId != null)
                        command.Parameters.Add("?managerId", MySqlDbType.Int32).Value = managerId.Value;

                    command.ExecuteNonQuery();
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return isSuccess;
        }

        #endregion

        #region Section Methods

        public Section GetSection(int id)
        {
            Section section = null;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    string query = string.Format("SELECT * FROM {0} WHERE Id = {1};", SECTIONS_TABLE_NAME, id);
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = $"{reader["Name"]}";
                            int departmentId = Convert.ToInt32($"{reader["DepartmentID"]}");
                            int authorId = Convert.ToInt32($"{reader["AuthorID"]}");

                            Department department = null;

                            if (this.IsRecursiveSelectionEnable)
                            {
                                department = this.GetDepartment(departmentId);
                                section = new Section(id, name, department, authorId);
                            }
                            else
                            {
                                section = new Section(id, name, departmentId, authorId);
                            }
                        }
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
                    connection.Close();
                }
            }

            return section;
        }

        public List<Section> GetSectionList()
        {
            List<Section> sectionList = new List<Section>();
            bool isSuccess = true;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(string.Format("SELECT * FROM {0};", SECTIONS_TABLE_NAME), connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32($"{reader["Id"]}");
                            string name = $"{reader["Name"]}";
                            int departmentId = Convert.ToInt32($"{reader["DepartmentID"]}");
                            int authorId = Convert.ToInt32($"{reader["AuthorID"]}");

                            Department department = null;
                            Section section = null;

                            if (this.IsRecursiveSelectionEnable)
                            {
                                department = this.GetDepartment(departmentId);
                                section = new Section(id, name, department, authorId);
                            }
                            else
                            {
                                section = new Section(id, name, departmentId, authorId);
                            }

                            sectionList.Add(section);
                            isSuccess &= true;
                        }
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return sectionList;
            else
                return null;
        }

        public Section AddSection(string name, int departmentId, int authorId)
        {
            Section section = null;
            bool isSuccess = true;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"INSERT INTO {0} " + "(Name, DepartmentID, AuthorID) " + "VALUES(?name, ?departmentId, ?authorId);", SECTIONS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("?departmentId", MySqlDbType.Int32).Value = departmentId;
                    command.Parameters.Add("?authorId", MySqlDbType.Int32).Value = authorId;
                    command.ExecuteNonQuery();

                    Department department = this.GetDepartment(departmentId);
                    Staff author = this.StaffRepository.GetStaff(authorId);


                    section = new Section((int)command.LastInsertedId, name, department, authorId);

                    isSuccess &= true;
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return section;
            else
                return null;
        }

        public Section AddSection(Section section)
        {
            return this.AddSection(section.Name, section.ParentUnitId, section.AuthorId);
        }

        public bool RemoveSection(int id)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"DELETE FROM {0} " + "WHERE Id=?id", SECTIONS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                    command.ExecuteNonQuery();
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return isSuccess;
        }

        public bool UpdateSection(int id, SectionForm form)
        {
            return this.UpdateSection(id, form.Name, form.ParentUnitId, form.AuthorId);
        }

        public bool UpdateSection(int id, string name = null, int? departmentId = null, int? authorId = null)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    List<string> queryPartList = new List<string>();

                    string sqlQuery = @"UPDATE {0} SET ";

                    if (!string.IsNullOrEmpty(name))
                        queryPartList.Add("Name=?name");
                    if (departmentId != null)
                        queryPartList.Add("DepartmentID=?departmentId");
                    if (authorId != null)
                        queryPartList.Add("AuthorID=?authorId");

                    sqlQuery += string.Join(", ", queryPartList);
                    sqlQuery += " WHERE Id=?id";

                    string query = string.Format(sqlQuery, SECTIONS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    // UserID
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                    // Name
                    if (!string.IsNullOrEmpty(name))
                        command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;

                    // DepartmentId
                    if (departmentId != null)
                        command.Parameters.Add("?departmentId", MySqlDbType.Int32).Value = departmentId.Value;

                    // AuthorId
                    if (authorId != null)
                        command.Parameters.Add("?authorId", MySqlDbType.Int32).Value = authorId.Value;

                    command.ExecuteNonQuery();
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return isSuccess;
        }

        #endregion

        #region Team Methods

        public Team GetTeam(int id)
        {
            Team team = null;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    string query = string.Format("SELECT * FROM {0} WHERE Id = {1};", TEAMS_TABLE_NAME, id);
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = $"{reader["Name"]}";
                            int departmentId = Convert.ToInt32($"{reader["DepartmentID"]}");
                            int sectionId = Convert.ToInt32($"{reader["SectionID"]}");
                            int teamLeadId = Convert.ToInt32($"{reader["TeamLeadID"]}");

                            Department department = null;
                            Section section = null;

                            if (this.IsRecursiveSelectionEnable)
                            {
                                department = this.GetDepartment(departmentId);
                                section = this.GetSection(sectionId);
                                team = new Team(id, name, section, department, teamLeadId);
                            }
                            else
                            {
                                team = new Team(id, name, sectionId, departmentId, teamLeadId);
                            }
                        }
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
                    connection.Close();
                }
            }

            return team;
        }

        public List<Team> GetTeamList()
        {
            List<Team> teamList = new List<Team>();
            bool isSuccess = true;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(string.Format("SELECT * FROM {0};", TEAMS_TABLE_NAME), connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32($"{reader["Id"]}");
                            string name = $"{reader["Name"]}";
                            int departmentId = Convert.ToInt32($"{reader["DepartmentID"]}");
                            int sectionId = Convert.ToInt32($"{reader["SectionID"]}");
                            int teamLeadId = Convert.ToInt32($"{reader["TeamLeadID"]}");

                            Department department = null;
                            Section section = null;
                            Team team = null;

                            if (this.IsRecursiveSelectionEnable)
                            {
                                department = this.GetDepartment(departmentId);
                                section = this.GetSection(sectionId);
                                team = new Team(id, name, section, department, teamLeadId);
                            }
                            else
                            {
                                team = new Team(id, name, sectionId, departmentId, teamLeadId);
                            }

                            teamList.Add(team);
                            isSuccess &= true;
                        }
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return teamList;
            else
                return null;
        }

        public Team AddTeam(string name, int departmentId, int sectionId, int teamLeadId)
        {
            Team team = null;
            bool isSuccess = true;

            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"INSERT INTO {0} " + "(Name, DepartmentID, SectionID, TeamLeadID) " + "VALUES(?name, ?departmentId, ?sectionId, ?teamLeadId);", TEAMS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("?departmentId", MySqlDbType.Int32).Value = departmentId;
                    command.Parameters.Add("?sectionId", MySqlDbType.Int32).Value = sectionId;
                    command.Parameters.Add("?teamLeadId", MySqlDbType.Int32).Value = teamLeadId;
                    command.ExecuteNonQuery();

                    Department department = this.GetDepartment(departmentId);
                    Section section = this.GetSection(sectionId);
                    Staff teamLead = this.StaffRepository.GetStaff(teamLeadId);

                    team = new Team((int)command.LastInsertedId, name, section, department, teamLeadId);

                    isSuccess &= true;
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (isSuccess)
                return team;
            else
                return null;
        }

        public Team AddTeam(Team team)
        {
            return this.AddTeam(team.Name, team.DepartmentId, team.SectionId, team.TeamLeadId);
        }

        public bool RemoveTeam(int id)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    string query = string.Format(@"DELETE FROM {0} " + "WHERE Id=?id", TEAMS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                    command.ExecuteNonQuery();
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return isSuccess;
        }

        public bool UpdateTeam(int id, TeamForm form)
        {
            return this.UpdateTeam(id, form.Name, form.DepartmentId, form.SectionId, form.TeamLeadId);
        }

        public bool UpdateTeam(int id, string name, int? departmentId, int? sectionId, int? teamLeadId)
        {
            bool isSuccess = true;
            using (MySqlConnection connection = new MySqlConnection(this.CONNECTION_STRING))
            {
                try
                {
                    List<string> queryPartList = new List<string>();

                    string sqlQuery = @"UPDATE {0} SET ";

                    if (!string.IsNullOrEmpty(name))
                        queryPartList.Add("Name=?name");
                    if (departmentId != null)
                        queryPartList.Add("DepartmentID=?departmentId");
                    if (sectionId != null)
                        queryPartList.Add("SectionID=?sectionId");
                    if (teamLeadId != null)
                        queryPartList.Add("TeamLeadID=?teamLeadId");

                    sqlQuery += string.Join(", ", queryPartList);
                    sqlQuery += " WHERE Id=?id";

                    string query = string.Format(sqlQuery, TEAMS_TABLE_NAME);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = query
                    };

                    // UserID
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

                    // Name
                    if (!string.IsNullOrEmpty(name))
                        command.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;

                    // DepartmentId
                    if (departmentId != null)
                        command.Parameters.Add("?departmentId", MySqlDbType.Int32).Value = departmentId.Value;

                    // SectionId
                    if (sectionId != null)
                        command.Parameters.Add("?sectionId", MySqlDbType.Int32).Value = sectionId.Value;

                    // TeamLeadId
                    if (teamLeadId != null)
                        command.Parameters.Add("?teamLeadId", MySqlDbType.Int32).Value = teamLeadId.Value;

                    command.ExecuteNonQuery();
                }
                catch (MySqlException mysqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database'e bağlanılamıyor! \n\r" + mysqlEx.StackTrace);
                    isSuccess = false;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    isSuccess = false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return isSuccess;
        }

        #endregion
    }
}
