using System;
using System.Collections.Generic;
using Ertis.Core.Human;
using Ertis.WebService.Models;
using MySql.Data.MySqlClient;

namespace Ertis.WebService.Dao.Contracts
{
    public interface IStaffRepository
    {
        // Staff
        Staff GetStaff(int id);
        List<Staff> GetStaffList();
        Staff AddStaff(StaffForm form);
        bool RemoveStaff(int id);
        bool UpdateStaff(int id, StaffForm form);
        bool UpdateStaff(
            int id,
            int? credentialId = null,
            string name = null,
            string surname = null,
            string emailAddress = null,
            string phoneNumber = null,
            DateTime? birthDate = null,
            int? departmentId = null,
            int? sectionId = null,
            int? teamId = null,
            int? positionId = null,
            MySqlConnection connection = null);
        
        // Position
        Position GetPosition(int id);
        List<Position> GetPositionList();
        Position AddPosition(PositionForm form);
        bool UpdatePosition(int id, PositionForm form);
        bool RemovePosition(int id);
    }
}
