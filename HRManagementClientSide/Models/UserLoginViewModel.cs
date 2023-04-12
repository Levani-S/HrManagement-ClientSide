using System.ComponentModel.DataAnnotations;

namespace HRManagementClientSide.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid Username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid Password.")]
        public string Password { get; set; }
    }
}
