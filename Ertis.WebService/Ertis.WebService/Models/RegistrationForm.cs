using System;
using Ertis.Core.Profile;

namespace Ertis.WebService.Models
{
    public class RegistrationForm
    {
        public UserCard Card { get; set; }

        public string Password { get; set; }

        public bool? IsActive { get; set; }

        public int? UserRoleID { get; set; }

        public RegistrationForm()
        {
        }
    }
}
