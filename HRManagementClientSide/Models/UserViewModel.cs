using System.ComponentModel.DataAnnotations;

namespace HRManagementClientSide.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
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
        public Guid GenderId { get; set; }
    }
}
