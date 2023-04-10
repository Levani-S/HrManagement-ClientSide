using HRManagementClientSide.Models;

namespace HRManagementClientSide.Services.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetAllEmployee();
    }
}
