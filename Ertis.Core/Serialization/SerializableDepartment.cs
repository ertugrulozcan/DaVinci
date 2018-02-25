using Ertis.Core.Human;
using Ertis.Core.Organization;
using Ertis.Core.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableDepartment : SerializableOrganizationUnit, ISerializable<Department>
    {
        public int ManagerId { get; set; }

        [JsonIgnore]
        public List<Unit> UnitList { get; set; }

        public OrganizationUnit ParentUnit { get; set; }

        public SerializableDepartment()
        {
            this.UnitList = new List<Unit>();
        }

        public static Department Parse(SerializableDepartment department)
        {
            if (department == null)
                return null;

            return new Department(department.Id, department.Name, department.ManagerId)
            {
                ParentUnit = department.ParentUnit,
                StaffList = department.StaffList.Select(x => SerializableStaff.Parse(x)).ToList(),
                UnitList = department.UnitList
            };
        }

        public Department Deserialize(string json)
        {
            var department = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableDepartment>(json);
            return Parse(department);
        }

        public List<Department> DeserializeList(string jsonStr)
        {
            List<Department> list = new List<Department>();

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
                System.Diagnostics.Debug.WriteLine("SerializableDepartment.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
