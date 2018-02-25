using Ertis.Core.Organization;
using Ertis.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Services.Contracts
{
    public interface IOrganizationService
    {
        ObservableRangeCollection<Department> DepartmentList { get; }

        ObservableRangeCollection<Section> SectionList { get; }

        ObservableRangeCollection<Team> TeamList { get; }
    }
}
