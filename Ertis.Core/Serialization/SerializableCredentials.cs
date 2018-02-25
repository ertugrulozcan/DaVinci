using Ertis.Core.Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Serialization
{
    internal class SerializableCredentials : ISerializable<Credentials>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// Constructors
        /// </summary>
        public SerializableCredentials()
        {

        }

        public static Credentials Parse(SerializableCredentials credentials)
        {
            if (credentials == null)
                return null;

            return new Credentials(credentials.Id, credentials.Name, credentials.Surname)
            {
                EmailAddress = credentials.EmailAddress,
                PhoneNumber = credentials.PhoneNumber,
                BirthDate = credentials.BirthDate
            };
        }

        public Credentials Deserialize(string json)
        {
            var credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableCredentials>(json);
            return Parse(credentials);
        }

        public List<Credentials> DeserializeList(string jsonStr)
        {
            List<Credentials> list = new List<Credentials>();

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
                System.Diagnostics.Debug.WriteLine("SerializableCredentials.DeserializeList() error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("JsonString :");
                System.Diagnostics.Debug.WriteLine(jsonStr);
            }

            return list;
        }
    }
}
