using System;
using System.Collections.Generic;
using Ertis.Core.Profile;

namespace Ertis.WebService.Dao.Contracts
{
    public interface IUserRepository
    {
        User Get(int id);
        List<User> GetList();
        User Add(UserCard card, string passwordHash);
        User Add(UserCard card, string passwordHash, bool isActive, UserRole role);
        bool Update(int id, UserCard userCard);
        bool Update(int id, string name = null, string surname = null, DateTime? birthDate = null, string phoneNumber = null, bool? isActive = null);
        bool Remove(int id);
        bool Remove(User user);
        string GetPasswordHash(int id);
    }
}
