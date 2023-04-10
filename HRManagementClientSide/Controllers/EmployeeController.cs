using HRManagementClientSide.Models;
using HRManagementClientSide.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementClientSide.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(ILogger<EmployeeController> logger,IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }
        public IActionResult Employee()
        {
            return View("~/Views/Employee/EmployeeList.cshtml");
        }
        public async Task<IActionResult> EmployeeList()
        {
            var result = await _employeeService.GetAllEmployee();
            if (result == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View(result);
        }
    }
}

