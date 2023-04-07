namespace HRManagementClientSide.Models
{
    public class UserLoginResponseViewModel : UserViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
