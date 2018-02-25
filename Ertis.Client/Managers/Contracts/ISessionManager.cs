using Ertis.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Client.Managers.Contracts
{
    public interface ISessionManager
    {
        Session CurrentSession { get; }

        Session CreateSession(int userID, string token);
    }
}
