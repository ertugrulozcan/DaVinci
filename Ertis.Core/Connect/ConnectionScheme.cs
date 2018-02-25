using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Core.Connect
{
    public class ConnectionScheme
    {
        #region Constants

        private const string LIVE_SERVER_URL = @"https://ertis.azurewebsites.net/";
        private const string DEMO_SERVER_URL = @"https://demo.azurewebsites.net/";
        private const string TEST_SERVER_URL = @"https://test.azurewebsites.net/";
        private const string LOCALHOST_SERVER_URL = @"http://localhost:5000/";

        #endregion

        #region Fields

        private static ConnectionScheme self;
        private ServerType selectedServerType = ServerType.Live;

        #endregion

        #region Properties

        public static ConnectionScheme Current
        {
            get
            {
                if (self == null)
                    self = new ConnectionScheme();

                return self;
            }
        }

        public ServerType SelectedServerType
        {
            get
            {
                return selectedServerType;
            }

            set
            {
                selectedServerType = value;
            }
        }

        public string ServerURL
        {
            get
            {
                switch (this.SelectedServerType)
                {
                    default:
                    case ServerType.Live:
                        return LIVE_SERVER_URL;
                    case ServerType.Demo:
                        return DEMO_SERVER_URL;
                    case ServerType.Test:
                        return TEST_SERVER_URL;
                    case ServerType.Localhost:
                        return LOCALHOST_SERVER_URL;
                }
            }
        }

        public string LoginURL
        {
            get
            {
                return this.ServerURL + @"api/login";
            }
        }

        public string UserServiceURL
        {
            get
            {
                return this.ServerURL + @"api/users";
            }
        }

        public string StaffServiceURL
        {
            get
            {
                return this.ServerURL + @"api/staff";
            }
        }

        public string PositionServiceURL
        {
            get
            {
                return this.ServerURL + @"api/staff/positions";
            }
        }

        public string DepartmentServiceURL
        {
            get
            {
                return this.ServerURL + @"api/organization/departments";
            }
        }

        public string SectionServiceURL
        {
            get
            {
                return this.ServerURL + @"api/organization/sections";
            }
        }

        public string TeamServiceURL
        {
            get
            {
                return this.ServerURL + @"api/organization/teams";
            }
        }

        #endregion

        #region Constructors

        private ConnectionScheme()
        {

        }

        #endregion

        #region Enums

        public enum ServerType
        {
            Live,
            Demo,
            Test,
            Localhost
        }

        #endregion
    }
}
