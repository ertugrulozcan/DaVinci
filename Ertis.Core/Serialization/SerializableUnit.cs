using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableUnit : SerializableOrganizationUnit
    {
        public SerializableDepartment Department { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializableUnit()
        {

        }
    }
}
