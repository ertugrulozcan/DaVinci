using System;
namespace Ertis.WebService.Models
{
    public class TeamForm
    {
        public string Name { get; set; }

        public int? TeamLeadId { get; set; }

        public int? DepartmentId { get; set; }

        public int? SectionId { get; set; }

        public TeamForm()
        {
            
        }
    }
}
