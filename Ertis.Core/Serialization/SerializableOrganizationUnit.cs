using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableOrganizationUnit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SerializableStaff> StaffList { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public SerializableOrganizationUnit()
        {
            this.StaffList = new List<SerializableStaff>();
        }
    }
}
