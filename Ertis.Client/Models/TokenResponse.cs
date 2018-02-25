using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Client.Models
{
    public class TokenResponse
    {
        /*
        
        // Sample
        {
            "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlcnR1Z3J1bC5vemNhbkBwcm9qZWN0ZXJ0aXMuY29tIiwianRpIjoiMzVjNzgzZjYtOTEyYi00ZTdjLWI0NTYtMjExYmI3YTUwNDc0IiwiaWF0IjoxNTA5ODAxNDQ0LCJuYmYiOjE1MDk4MDE0NDQsImV4cCI6MTUwOTgwMTc0NCwiaXNzIjoiaHR0cDovL2FzdHJvdHVyZi5henVyZXdlYnNpdGVzLm5ldC8iLCJhdWQiOiJodHRwOi8vYXN0cm90dXJmLmF6dXJld2Vic2l0ZXMubmV0LyJ9.U9HXAI_dP_gxKfEwcONu_E9bC0xwW-SRM7c7zW9Ojr8",
            "expires_in": 300,
            "user_id": 1004
        }

        */
        
        #region Properties

        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public int UserID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        private TokenResponse()
        {

        }

        #endregion

        #region Methods

        public static TokenResponse Create(string json)
        {
            var definition = new { access_token = "", expires_in = "", user_id = "" };
            var anonymousTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, definition);

            return new TokenResponse()
            {
                AccessToken = anonymousTokenResponse.access_token,
                ExpiresIn = Int32.Parse(anonymousTokenResponse.expires_in),
                UserID = Int32.Parse(anonymousTokenResponse.user_id)
            };
        }

        public override string ToString()
        {
            return this.AccessToken;
        }

        #endregion
    }
}
