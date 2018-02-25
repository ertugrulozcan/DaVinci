using Ertis.Core.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableTeam : SerializableUnit, ISerializable<Team>
    {
        public int TeamLeadId { get; set; }

        public SerializableSection Section { get; set; }

        public SerializableOrganizationUnit ParentUnit { get; set; }

        public int SectionId { get; set; }

        public int DepartmentId { get; set; }

        /// <summary>
        /// Constructors
        /// </summary>
        public SerializableTeam()
        {

        }

        public static Team Parse(SerializableTeam team)
        {
            if (team == null)
                return null;

            OrganizationUnit parentUnit = null;
            if (team.ParentUnit is SerializableSection)
                parentUnit = SerializableSection.Parse(team.ParentUnit as SerializableSection);
            if (team.ParentUnit is SerializableDepartment)
                parentUnit = SerializableDepartment.Parse(team.ParentUnit as SerializableDepartment);

            return new Team(team.Id, team.Name, team.SectionId, team.DepartmentId, team.TeamLeadId)
            {
                ParentUnit = parentUnit,
                StaffList = team.StaffList.Select(x => SerializableStaff.Parse(x)).ToList(),
            };
        }

        public Team Deserialize(string json)
        {
            var team = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableTeam>(json);
            return Parse(team);
        }

        public List<Team> DeserializeList(string jsonStr)
        {
            List<Team> list = new List<Team>();

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
                System.Diagnostics.Debug.WriteLine("SerializableTeam.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
