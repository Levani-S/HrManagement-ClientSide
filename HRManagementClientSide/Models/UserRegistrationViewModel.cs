using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HRManagementClientSide.Models
{
    public class UserRegistrationViewModel
    {
        [StringLength(11, MinimumLength = 11, ErrorMessage = "ID Number must be 11 characters.")]
        public string IdentifyNumber { get; set; }

        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public Guid GenderId { get; set; }
    }
}
