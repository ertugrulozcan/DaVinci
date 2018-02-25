using System.ComponentModel.DataAnnotations;

namespace Ertis.WebService.Auth
{
    public class Credentials
    {
        [Required]
        [EmailAddress]
        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}