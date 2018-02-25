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
    public static class SerializationHelper
    {
        public static Staff DeserializeStaff(string json)
        {
            var serializer = new SerializableStaff();
            return serializer.Deserialize(json);
        }

        public static List<Staff> DeserializeStaffList(string json)
        {
            var serializer = new SerializableStaff();
            return serializer.DeserializeList(json);
        }

        public static Department DeserializeDepartment(string json)
        {
            var serializer = new SerializableDepartment();
            return serializer.Deserialize(json);
        }

        public static List<Department> DeserializeDepartmentList(string json)
        {
            var serializer = new SerializableDepartment();
            return serializer.DeserializeList(json);
        }

        public static Section DeserializeSection(string json)
        {
            var serializer = new SerializableSection();
            return serializer.Deserialize(json);
        }

        public static List<Section> DeserializeSectionList(string json)
        {
            var serializer = new SerializableSection();
            return serializer.DeserializeList(json);
        }

        public static Team DeserializeTeam(string json)
        {
            var serializer = new SerializableTeam();
            return serializer.Deserialize(json);
        }

        public static List<Team> DeserializeTeamList(string json)
        {
            var serializer = new SerializableTeam();
            return serializer.DeserializeList(json);
        }
    }
}
