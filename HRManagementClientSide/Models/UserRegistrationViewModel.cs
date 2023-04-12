using System.ComponentModel.DataAnnotations;

namespace HRManagementClientSide.Models
{
    public class UserRegistrationViewModel
    {
        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "ID Number must be 11 characters.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ID Number must contain only numbers.")]
        public string IdentifyNumber { get; set; }

        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
        public Guid GenderId { get; set; }
    }
}
