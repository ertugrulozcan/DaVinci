using Ertis.Core.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableUser : ISerializable<User>
    {
        public int Id { get; set; }

        public SerializableUserCard Card { get; set; }

        public bool IsActive { get; set; }

        public UserRole UserRole { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializableUser()
        {

        }

        public static User Parse(SerializableUser user)
        {
            if (user == null)
                return null;
            
            return new User(user.Id, SerializableUserCard.Parse(user.Card))
            {
                IsActive = user.IsActive,
                UserRole = user.UserRole
            };
        }

        public User Deserialize(string json)
        {
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableUser>(json);
            return Parse(user);
        }

        public List<User> DeserializeList(string jsonStr)
        {
            List<User> list = new List<User>();

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
                System.Diagnostics.Debug.WriteLine("SerializableUser.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }
            
            return list;
        }
    }
}
