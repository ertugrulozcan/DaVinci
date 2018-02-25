using System;
using Ertis.Core.Profile;

namespace Ertis.Core.Server
{
    public class Session
    {
        #region Fields

        private User sessionUser;
        private string token;
        private string appID;
        private DateTime creationTime;
        private DateTime lastTime;

        #endregion

        #region Properties

        public User SessionUser
        {
            get
            {
                return this.sessionUser;
            }

            private set
            {
                this.sessionUser = value;
            }
        }

        public string Token
        {
            get
            {
                return this.token;
            }

            private set
            {
                this.token = value;
            }
        }

        public string AppID
        {
            get
            {
                return this.appID;
            }

            private set
            {
                this.appID = value;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return this.creationTime;
            }
        }

        public DateTime LastTime
        {
            get
            {
                if (this.lastTime == null)
                    return DateTime.Now;

                return this.lastTime;
            }
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                if (this.Status == SessionStatus.Active)
                    return DateTime.Now - this.CreationTime;
                else
                    return this.LastTime - this.CreationTime;
            }
        }

        public SessionStatus Status { get; private set; }

        #endregion

        #region Events

        public event EventHandler OnSessionStarted;
        public event EventHandler OnSessionEnded;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="token">Token.</param>
        /// <param name="appID">App ıd.</param>
        private Session(User user, string token, string appID)
        {
            this.SessionUser = user;
            this.Token = token;
            this.AppID = appID;
        }

        #endregion

        #region Methods

        public static Session Create(User user, string token, string appID)
        {
            Session session = new Session(user, token, appID);
            session.Start();

            return session;
        }

        public void Start()
        {
            this.Status = SessionStatus.Active;
            this.creationTime = DateTime.Now;

            if (this.OnSessionStarted != null)
                this.OnSessionStarted(this, new EventArgs());
        }

        public void Terminate()
        {
            this.Status = SessionStatus.Terminated;
            this.lastTime = DateTime.Now;

            if (this.OnSessionEnded != null)
                this.OnSessionEnded(this, new EventArgs());
        }

        #endregion

        #region Enums

        public enum SessionStatus
        {
            Active,
            Expired,
            Terminated
        }

        #endregion
    }
}
