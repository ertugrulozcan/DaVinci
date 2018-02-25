using Ertis.Client.Managers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Client.Services.Contracts;
using Ertis.Core.Server;

namespace Ertis.Client.Managers
{
    public class SessionManager : ISessionManager
    {
        #region Services

        private readonly IErtisWebService ertisWebService;

        #endregion

        #region Fields

        private Session currentSession;

        #endregion

        #region Properties

        public Session CurrentSession
        {
            get
            {
                return currentSession;
            }

            private set
            {
                this.currentSession = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ertisWebService"></param>
        public SessionManager(IErtisWebService ertisWebService)
        {
            this.ertisWebService = ertisWebService;
        }

        #endregion

        #region Methods

        public Session CreateSession(int userID, string token)
        {
            var user = ertisWebService.GetUser(userID);
            if (user != null)
            {
                this.CurrentSession = Session.Create(user, token, Application.AppID.GetAppID());
            }

            return this.CurrentSession;
        }

        #endregion
    }
}
