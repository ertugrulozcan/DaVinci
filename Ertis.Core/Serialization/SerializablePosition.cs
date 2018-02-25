using Ertis.Core.Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializablePosition : ISerializable<Position>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializablePosition()
        {

        }

        public static Position Parse(SerializablePosition position)
        {
            if (position == null)
                return null;

            return new Position(position.Id, position.Name)
            {
                Description = position.Description
            };
        }

        public Position Deserialize(string json)
        {
            var position = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializablePosition>(json);
            return Parse(position);
        }

        public List<Position> DeserializeList(string jsonStr)
        {
            List<Position> list = new List<Position>();

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
                System.Diagnostics.Debug.WriteLine("SerializablePosition.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
