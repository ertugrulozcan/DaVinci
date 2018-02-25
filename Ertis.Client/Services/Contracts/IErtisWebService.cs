using Ertis.Client.Models;
using Ertis.Core.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Core.Human;
using Ertis.Core.Organization;

namespace Ertis.Client.Services.Contracts
{
    public interface IErtisWebService
    {
        /// 
        /// Authentication
        /// 
        string Token { get; }
        void GetCompany();
        ResponseResult Login(string username, string password);

        /// 
        /// User
        /// 
        User GetUser(int userID);
        List<User> GetUserList();
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool ActivateUser(User user);
        bool DeactivateUser(User user);

        /// 
        /// Staffs
        /// 
        Staff GetStaff(int staffID);
        List<Staff> GetStaffList();
        bool UpdateStaff(Staff staff);
        bool DeleteStaff(Staff staff);

        //
        // Positions
        // 
        Position GetPosition(int positionId);
        List<Position> GetPositionList();
        
        /// 
        /// Departments
        /// 
        Department GetDepartment(int departmentID);
        List<Department> GetDepartmentList();
        bool UpdateDepartment(Department department);
        bool DeleteDepartment(Department department);
        
        /// 
        /// Sections
        /// 
        Section GetSection(int sectionID);
        List<Section> GetSectionList();
        bool UpdateSection(Section section);
        bool DeleteSection(Section section);

        /// 
        /// Teams
        /// 
        Team GetTeam(int teamID);
        List<Team> GetTeamList();
        bool UpdateTeam(Team team);
        bool DeleteTeam(Team team);
    }
}
