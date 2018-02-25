using System;
namespace Ertis.WebService.Models
{
    public class SectionForm
    {
        public string Name { get; set; }

        public int? AuthorId { get; set; }

        public int? ParentUnitId { get; set; }

        public SectionForm(string name, int? managerId, int? parentUnitId)
        {
            this.Name = name;
            this.AuthorId = managerId;
            this.ParentUnitId = parentUnitId;
        }
    }
}
