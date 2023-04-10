using HRManagementClientSide.Models;
using HRManagementClientSide.Services.ServiceInterfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HRManagementClientSide.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IAccountsService _accountsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string BaseUrl = "https://localhost:7074/api/";
        public EmployeeService(IHttpContextAccessor httpContextAccessor, IAccountsService accountsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountsService = accountsService;
        }
        public async Task<List<EmployeeModel>> GetAllEmployee()
        {
            var accessToken = _httpContextAccessor.HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                return null;
            }
            if (!await _accountsService.CheckTokenValidation(accessToken))
            {
                var result = await _accountsService.GetNewTokens();
                if (!result)
                {
                    return null;
                }
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var result = await client.GetAsync("Employee/AllEmployee");

                List<EmployeeModel> allEmp = new List<EmployeeModel>();
                if (result.IsSuccessStatusCode)
                {
                    var returnedData = result.Content.ReadAsStringAsync().Result;
                    allEmp = JsonConvert.DeserializeObject<List<EmployeeModel>>(returnedData);
                }
                return allEmp;
            }
        }
    }
}

