using System.Collections.Generic;
using Ertis.WebService.Models;
using Ertis.Core.Profile;
using Ertis.Core.Data;

namespace Ertis.WebService.Services.Contracts
{
    public interface IUserService
    {
        Result Register(UserCard usercard, string password, bool isActive, UserRole userRole);

		User GetUser(int id);

        User GetUser(string userIdintifier);

		List<User> GetUserList();
		
        Result Update(int id, UserCard userCard);
		
        Result Remove(int id);

        bool RemoveCredentials(int id);

        string GetPasswordHash(User user);

        bool ValidatePassword(User user, string password);

        Result SetIsActive(int userId, bool isActive);
	}
}