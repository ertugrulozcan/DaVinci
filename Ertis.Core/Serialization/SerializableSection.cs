using Ertis.Core.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableSection : SerializableUnit, ISerializable<Section>
    {
        public int AuthorId { get; set; }

        public List<SerializableUnit> SubUnits { get; set; }

        public SerializableOrganizationUnit ParentUnit { get; set; }

        public int ParentUnitId { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public SerializableSection()
        {
            this.SubUnits = new List<SerializableUnit>();
        }

        public static Section Parse(SerializableSection section)
        {
            if (section == null)
                return null;

            List<Unit> subUnits = new List<Unit>();
            foreach (var subUnit in section.SubUnits)
            {
                if (subUnit is SerializableSection)
                    subUnits.Add(SerializableSection.Parse(subUnit as SerializableSection));

                if (subUnit is SerializableTeam)
                    subUnits.Add(SerializableTeam.Parse(subUnit as SerializableTeam));
            }

            OrganizationUnit parentUnit = null;
            if (section.ParentUnit is SerializableSection)
                parentUnit = SerializableSection.Parse(section.ParentUnit as SerializableSection);
            if (section.ParentUnit is SerializableDepartment)
                parentUnit = SerializableDepartment.Parse(section.ParentUnit as SerializableDepartment);

            return new Section(section.Id, section.Name, section.ParentUnitId, section.AuthorId)
            {
                Department = SerializableDepartment.Parse(section.Department),
                ParentUnit = parentUnit,
                StaffList = section.StaffList.Select(x => SerializableStaff.Parse(x)).ToList(),
                SubUnits = subUnits
            };
        }

        public Section Deserialize(string json)
        {
            var section = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableSection>(json);
            return Parse(section);
        }

        public List<Section> DeserializeList(string jsonStr)
        {
            List<Section> list = new List<Section>();

            try
            {
                var json = Newtonsoft.Json.Linq.JArray.Parse(jsonStr);
                foreach (var c in json.Children())
                {
                    list.Add(this.Deserialize(c.ToString()));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SerializableSection.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
