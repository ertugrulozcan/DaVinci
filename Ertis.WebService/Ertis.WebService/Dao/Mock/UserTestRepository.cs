using System;
using System.Linq;
using System.Collections.Generic;
using Ertis.Core.Profile;
using Ertis.WebService.Dao.Contracts;

namespace Ertis.WebService.Dao.Mock
{
    public class UserTestRepository : IUserRepository
    {
        private readonly int ID_DEFAULT = 1000;

        public Dictionary<int, User> UserDictionary { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserTestRepository()
        {
            this.UserDictionary = new Dictionary<int, User>();

            this.Add(new UserCard("Ertuğrul", "Özcan"));
            this.Add(new UserCard("Gözde", "Kaval"));
        }

        public User Add(UserCard userCard, string passwordHash = "")
        {
            int id = ID_DEFAULT + this.UserDictionary.Count;
            User user = new User(id, userCard);
            this.UserDictionary.Add(id, user);

            return user;
        }

        public User Add(UserCard userCard, string passwordHash, bool isActive, UserRole role)
        {
            int id = ID_DEFAULT + this.UserDictionary.Count;
            User user = new User(id, userCard);
            this.UserDictionary.Add(id, user);

            return user;
        }

        public User Get(int id)
        {
            if (this.UserDictionary.ContainsKey(id))
                return this.UserDictionary[id];
            else
                return null;
        }

        public List<User> GetList()
        {
            return this.UserDictionary.Values.ToList();
        }

        public bool Remove(int id)
        {
            if (this.UserDictionary.ContainsKey(id))
                return this.UserDictionary.Remove(id);

            return false;
        }

        public bool Remove(User user)
        {
            if (user != null)
                return this.Remove(user.Id);

            return false;
        }

        public bool Update(int id, UserCard userCard)
        {
            try
            {
                if (this.UserDictionary.ContainsKey(id))
                    this.UserDictionary[id].Override(userCard);
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public string GetPasswordHash(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, string name = null, string surname = null, DateTime? birthDate = null, string phoneNumber = null, bool? isActive = null)
        {
            throw new NotImplementedException();
        }
    }
}
