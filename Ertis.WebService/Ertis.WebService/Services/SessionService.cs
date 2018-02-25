using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ertis.WebService.Models;
using Ertis.WebService.Services.Contracts;
using Ertis.Core.Profile;
using Ertis.Core.Server;

namespace Ertis.WebService.Services
{
    public class SessionService : ISessionService
    {
        #region Services



        #endregion

        #region Fields

        private Dictionary<int, Session> sessionList;
        private ReadOnlyDictionary<int, Session> sessionDictionary;

        #endregion

        #region Properties

        public ReadOnlyDictionary<int, Session> SessionDictionary
        {
            get
            {
                return this.sessionDictionary;
            }

            private set
            {
                this.sessionDictionary = value;
            }
        }

        #endregion

        #region Constructors

        public SessionService()
        {
            this.sessionList = new Dictionary<int, Session>();
            this.SessionDictionary = new ReadOnlyDictionary<int, Session>(this.sessionList);
        }

        #endregion

        #region Methods

        public Session CreateSession(User user, string token, string appID)
        {
            if (user != null)
            {
                if (this.sessionDictionary.ContainsKey(user.Id))
                {
                    return this.sessionDictionary[user.Id];
                }
                else
                {
                    var session = Session.Create(user, token, appID);
                    this.sessionList.Add(user.Id, session);

                    return session;
                }
            }

            return null;
        }

        #endregion
    }
}
