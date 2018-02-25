using Ertis.Core.Profile;
using Ertis.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Services.Contracts
{
    public interface IUserService
    {
        ObservableRangeCollection<User> UserList { get; }

        void UpdateUser(User editingUser);

        void DeleteUser(User editingUser);

        void ActivateUser(User user);

        void DeactivateUser(User user);

        event EventHandler OnUserListFetched;
    }
}
