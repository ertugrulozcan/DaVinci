using Ertis.Core.Human;
using Ertis.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Services.Contracts
{
    public interface IStaffService
    {
        // Staffs
        ObservableRangeCollection<Staff> StaffList { get; }
        void UpdateStaff(Staff editingStaff);
        void DeleteStaff(Staff editingStaff);

        // Positions
        ObservableRangeCollection<Position> PositionList { get; }

        // Events
        event EventHandler OnStaffListFetched;
        event EventHandler OnPositionListFetched;
    }
}
