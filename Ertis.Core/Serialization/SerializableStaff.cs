using Ertis.Core.Human;
using Ertis.Core.Organization;
using Ertis.Core.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableStaff : ISerializable<Staff>
    {
        public int Id { get; set; }

        public SerializableCredentials UserCredentials { get; set; }

        public int CredentialsId { get; set; }

        public SerializableUser ErtisUser { get; set; }

        public int UserId { get; set; }

        public SerializablePosition Position { get; set; }

        public int PositionId { get; set; }

        public SerializableDepartment Department { get; set; }

        public int DepartmentId { get; set; }

        public SerializableSection Section { get; set; }

        public int SectionId { get; set; }

        public SerializableTeam Team { get; set; }

        public int TeamId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializableStaff()
        {

        }

        public static Staff Parse(SerializableStaff staff)
        {
            if (staff == null)
                return null;

            UserCard userCard = null;
            User user = null;
            Position position = null;
            Credentials credentials = null;
            Department department = null;
            Section section = null;
            Team team = null;

            if (staff.ErtisUser != null && staff.ErtisUser.Card != null)
                userCard = SerializableUserCard.Parse(staff.ErtisUser.Card);

            if (staff.ErtisUser != null)
                user = SerializableUser.Parse(staff.ErtisUser);

            if (staff.Position != null)
                position = SerializablePosition.Parse(staff.Position);

            if (staff.UserCredentials != null)
                credentials = SerializableCredentials.Parse(staff.UserCredentials);

            if (staff.Department != null)
                department = SerializableDepartment.Parse(staff.Department);

            if (staff.Section != null)
                section = SerializableSection.Parse(staff.Section);

            if (staff.Team != null)
                team = SerializableTeam.Parse(staff.Team);

            return new Staff(staff.Id, user, position)
            {
                PositionId = staff.PositionId,
                UserId = staff.UserId,
                CredentialsId = staff.CredentialsId,
                UserCredentials = credentials,
                DepartmentId = staff.DepartmentId,
                Department = department,
                Section = section,
                SectionId = staff.SectionId,
                Team = team,
                TeamId = staff.TeamId
            };
        }

        public Staff Deserialize(string json)
        {
            var staff = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableStaff>(json);
            return Parse(staff);
        }

        public List<Staff> DeserializeList(string jsonStr)
        {
            List<Staff> list = new List<Staff>();

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
                System.Diagnostics.Debug.WriteLine("SerializableStaff.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
