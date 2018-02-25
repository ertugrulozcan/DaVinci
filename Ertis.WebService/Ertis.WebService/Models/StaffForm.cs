using System;
namespace Ertis.WebService.Models
{
    public class StaffForm
    {
        public int? CredentialID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? UserID { get; set; }

        public int? PositionID { get; set; }

        public int? DepartmentID { get; set; }

        public int? SectionID { get; set; }

        public int? TeamID { get; set; }

        public StaffForm()
        {
        }
    }
}
