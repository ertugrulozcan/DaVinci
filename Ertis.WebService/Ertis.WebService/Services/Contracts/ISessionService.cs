using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ertis.WebService.Models;
using Ertis.Core.Profile;
using Ertis.Core.Server;

namespace Ertis.WebService.Services.Contracts
{
    public interface ISessionService
    {
        ReadOnlyDictionary<int, Session> SessionDictionary { get; }

        Session CreateSession(User user, string token, string appID);
    }
}
