using Ertis.Core.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableUserCard : SerializableCredentials, ISerializable<UserCard>
    {
        public DateTime? JoinDate { get; set; }

        public SerializableUserCard()
        {

        }

        public static UserCard Parse(SerializableUserCard userCard)
        {
            if (userCard == null)
                return null;

            return new UserCard(userCard.Id)
            {
                Name = userCard.Name,
                Surname = userCard.Surname,
                EmailAddress = userCard.EmailAddress,
                PhoneNumber = userCard.PhoneNumber,
                BirthDate = userCard.BirthDate,
                JoinDate = userCard.JoinDate
            };
        }

        public new UserCard Deserialize(string json)
        {
            var userCard = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableUserCard>(json);
            return Parse(userCard);
        }

        public new List<UserCard> DeserializeList(string jsonStr)
        {
            List<UserCard> list = new List<UserCard>();

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
                System.Diagnostics.Debug.WriteLine("SerializableUserCard.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
