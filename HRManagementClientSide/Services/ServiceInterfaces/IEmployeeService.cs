using HRManagementClientSide.Models;

namespace HRManagementClientSide.Services.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetAllEmployee();
        Task<List<EmployeeModel>> GetFilteredEmployee(List<EmployeeFilter> filter);
        Task<EmployeeModel> CreateEmployee(EmployeeModel model);
        Task<EmployeeModel> GetEmployee(Guid id);
        Task<EmployeeModel> RemoveEmployee(Guid id);
        Task<(bool isSuccess, EmployeeModel employee, string errorMessage)> SaveEditedEmployee(EmployeeModel model, Guid id);
    }
}
