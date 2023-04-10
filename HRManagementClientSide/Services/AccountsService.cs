using HRManagementClientSide.Models;
using HRManagementClientSide.Services.ServiceInterfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HRManagementClientSide.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string BaseUrl = "https://localhost:7074/";

        public AccountsService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public async Task<bool> LogInAsync(UserLoginViewModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                var postTask = client.PostAsJsonAsync("api/Accounts/LoginUser", model);
                var result = await postTask;

                if (result.IsSuccessStatusCode)
                {
                    var returnedData = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<UserLoginResponseViewModel>(returnedData);

                    _httpContextAccessor.HttpContext.Session.SetString("AccessToken", response.AccessToken);
                    _httpContextAccessor.HttpContext.Session.SetString("RefreshToken", response.RefreshToken);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> Register(UserRegistrationViewModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                var postTask = client.PostAsJsonAsync<UserRegistrationViewModel>("Accounts/RegisterUser", model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var loginModel = new UserLoginViewModel()
                    {
                        UserName = model.UserName,
                        Password = model.Password
                    };
                    var loginResult = await LogInAsync(loginModel);
                    if (loginResult)
                    {
                        return true;
                    }
                }
                return false;

            }
        }
        public Task<bool> CheckTokenValidation(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"])),
                ValidateLifetime = true
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) || jwtSecurityToken.ValidTo < DateTime.Now)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        public async Task<bool> GetNewTokens()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Session.GetString("RefreshToken");
            if (!await CheckTokenValidation(refreshToken))
            {
                return false;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var result = await client.PostAsJsonAsync<string>("Accounts/CreateNewTokens", refreshToken);

                if (result.IsSuccessStatusCode)
                {
                    var returnedData = result.Content.ReadAsStringAsync().Result;
                    UserLoginResponseViewModel res = JsonConvert.DeserializeObject<UserLoginResponseViewModel>(returnedData);

                    if (res.AccessToken == null && res.RefreshToken == null)
                    {
                        return false;
                    }
                    _httpContextAccessor.HttpContext?.Session.SetString("AccessToken", res.AccessToken);
                    _httpContextAccessor.HttpContext?.Session.SetString("RefreshToken", res.RefreshToken);
                    return true;
                }
                return false;
            }
        }
    }
}