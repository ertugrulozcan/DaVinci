using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    public interface ISerializable<T>
    {
        T Deserialize(string json);

        List<T> DeserializeList(string json);
    }
}
