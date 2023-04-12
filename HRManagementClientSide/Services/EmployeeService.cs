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
            var accessToken = _httpContextAccessor?.HttpContext?.Session?.GetString("AccessToken");
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return new List<EmployeeModel>();
            }
            if (!await _accountsService.CheckTokenValidation(accessToken))
            {
                var result = await _accountsService.GetNewTokens();
                if (!result)
                {
                    return new List<EmployeeModel>();
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
        public async Task<List<EmployeeModel>> GetFilteredEmployee(List<EmployeeFilter> filter)
        {
            var accessToken = await GetValidAccessToken();
            if (accessToken == null)
            {
                return new List<EmployeeModel>();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PostAsJsonAsync("Employee/Filter", filter);

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsAsync<List<EmployeeModel>>();
                }
                else
                {
                    return new List<EmployeeModel>();
                }
            }
        }
        private async Task<string> GetValidAccessToken()
        {
            var accessToken = _httpContextAccessor?.HttpContext?.Session?.GetString("AccessToken");
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return string.Empty;
            }
            if (!await _accountsService.CheckTokenValidation(accessToken))
            {
                var result = await _accountsService.GetNewTokens();
                if (!result)
                {
                    return string.Empty;
                }
            }
            return _httpContextAccessor?.HttpContext?.Session?.GetString("AccessToken") ?? string.Empty;
        }
        public async Task<EmployeeModel> GetEmployee(Guid id)
        {
            var accessToken = await GetValidAccessToken();
            if (accessToken == null)
            {
                return new EmployeeModel();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var result = await client.GetAsync($"Employee/GetEmployeeById/{id}");

                EmployeeModel emp = new EmployeeModel();
                if (result.IsSuccessStatusCode)
                {
                    var returnedData = result.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<EmployeeModel>(returnedData);
                }
                return emp;
            }
        }

        public async Task<EmployeeModel> RemoveEmployee(Guid id)
        {
            var accessToken = await GetValidAccessToken();
            if (accessToken == null)
            {
                return new EmployeeModel();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var result = await client.DeleteAsync($"Employee/DeleteEmployee/{id}");

                EmployeeModel emp = new EmployeeModel();
                if (result.IsSuccessStatusCode)
                {
                    var returnedData = result.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<EmployeeModel>(returnedData);
                }
                return emp;
            }
        }
        public async Task<(bool isSuccess, EmployeeModel employee, string errorMessage)> SaveEditedEmployee(EmployeeModel model, Guid id)
        {
            var accessToken = await GetValidAccessToken();
            if (accessToken == null)
            {
                return (false, null, "Unable to obtain an access token.");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var result = await client.PutAsJsonAsync($"Employee/EditEmployee/{id}", model);

                EmployeeModel emp = new EmployeeModel();
                if (result.IsSuccessStatusCode)
                {
                    var returnedData = result.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<EmployeeModel>(returnedData);
                    return (true, emp, null);
                }
                else
                {
                    var errorResponse = await result.Content.ReadAsStringAsync();
                    var errorObject = JsonConvert.DeserializeObject<dynamic>(errorResponse);
                    var errorMessage = Convert.ToString(errorObject.error);
                    return (false, null, errorMessage);
                }
            }
        }
        public async Task<EmployeeModel> CreateEmployee(EmployeeModel model)
        {
            var accessToken = await GetValidAccessToken();
            if (accessToken == null)
            {
                return new EmployeeModel();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var result = await client.PostAsJsonAsync($"Employee/AddEmployee", model);

                EmployeeModel emp = new EmployeeModel();
                if (result.IsSuccessStatusCode)
                {
                    var returnedData = result.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<EmployeeModel>(returnedData);
                }
                return emp;
            }
        }
    }
}

