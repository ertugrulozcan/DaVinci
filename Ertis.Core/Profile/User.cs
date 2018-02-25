using Ertis.Core.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ertis.Core.Profile
{
    public class User : ICloneable, IDisposable
    {
        #region Fields

        private int id;
        private UserRole userRole;
        private bool isActive = true;

        #endregion

        #region Properties

        public int Id
        {
            get
            {
                return id;
            }
            private set
            {
                id = value;
            }
        }

        [JsonIgnore]
        public string UserIdintifier
        {
            get
            {
                if (this.Card != null)
                    return this.Card.EmailAddress;
                else
                    return null;
            }
        }

        public UserCard Card { get; private set; }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }

        [JsonIgnore]
        public bool IsPassive
        {
            get
            {
                return !isActive;
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return this.Card.Name;
            }
        }

        [JsonIgnore]
        public string Surname
        {
            get
            {
                return this.Card.Surname;
            }
        }

        [JsonIgnore]
        public string FullName
        {
            get
            {
                return this.Card.FullName;
            }
        }

        public UserRole UserRole
        {
            get
            {
                return userRole;
            }
            set
            {
                userRole = value;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Private Constructor
        /// </summary>
        private User()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public User(int id)
        {
            this.Id = id;
            this.Card = new UserCard();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public User(int id, UserCard card)
        {
            this.Id = id;
            this.Card = card;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public User(int id, string name, string surname)
        {
            this.Id = id;
            this.Card = new UserCard(name, surname);
        }

        #endregion

        #region Methods

        public static User CreateFromJson(string json)
        {
            var userCardDefinition = new
            {
                name = "",
                surname = "",
                fullName = "",
                birthDate = "",
                joinDate = "",
                emailAddress = "",
                phoneNumber = ""
            };
            
            var userDefinition = new
            {
                id = "",
                card = userCardDefinition,
                userRole = 0,
                isActive = true
            };

            var anonymousUser = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, userDefinition);

            return new User()
            {
                Id = Int32.Parse(anonymousUser.id),
                Card = new UserCard()
                {
                    Name = anonymousUser.card.name,
                    Surname = anonymousUser.card.surname,
                    EmailAddress = anonymousUser.card.emailAddress,
                    PhoneNumber = anonymousUser.card.phoneNumber,
                    BirthDate = DateTime.Parse(anonymousUser.card.birthDate),
                    JoinDate = DateTime.Parse(anonymousUser.card.joinDate),
                },
                UserRole = ParseUserRole(anonymousUser.userRole),
                IsActive = anonymousUser.isActive
            };
        }

        public static UserRole ParseUserRole(int enumValue)
        {
            var userRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
            if (userRoles.Any(x => enumValue == (int)x))
                return userRoles.First(x => enumValue == (int)x);
            else
                return UserRole.Unrole;
        }

        public static List<User> ToListFromJson(string jsonStr)
        {
            List<User> userList = new List<User>();

            var json = Newtonsoft.Json.Linq.JArray.Parse(jsonStr);
            foreach (var c in json.Children())
            {
                userList.Add(CreateFromJson(c.ToString()));
            }

            return userList;
        }

        public void Override(User user)
        {
            this.Card = user.Card;
        }

        public void Override(UserCard userCard)
        {
            this.Card = userCard;
        }

        public override string ToString()
        {
            return string.Format("[User: Id={0}, Name={1}]", this.Id, this.FullName);
        }

        public object Clone()
        {
            return new User()
            {
                Id = this.Id,
                IsActive = this.IsActive,
                UserRole = this.UserRole,
                Card = new UserCard()
                {
                    Name = this.Name,
                    Surname = this.Surname,
                    EmailAddress = this.Card.EmailAddress,
                    PhoneNumber = this.Card.PhoneNumber,
                    BirthDate = this.Card.BirthDate,
                    JoinDate = this.Card.JoinDate
                }
            };
        }

        public void Dispose()
        {
            this.Card = null;
        }

        ~User()
        {
            this.Dispose();
        }

        #endregion
    }
}
