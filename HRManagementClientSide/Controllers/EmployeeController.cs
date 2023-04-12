using HRManagementClientSide.Models;
using HRManagementClientSide.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementClientSide.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }
        public IActionResult AddEmployee()
        {
            return View();
        }
        public async Task<IActionResult> EmployeeList(List<EmployeeFilter>? filters = null)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("AccessToken")))
            {
                return RedirectToAction("Index","Home");
            }
            List<EmployeeModel> result;
            if (filters == null || filters.Count == 0)
            {
                result = await _employeeService.GetAllEmployee();
            }
            else
            {
                result = await _employeeService.GetFilteredEmployee(filters);
            }

            var viewModel = new EmployeeViewModel
            {
                Employees = result,
                Filters = filters?.Count >= 2 ? filters : new List<EmployeeFilter>
        {
            new EmployeeFilter(),
            new EmployeeFilter()
        }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> RemoveEmployee(Guid id)
        {
            await _employeeService.RemoveEmployee(id);
            return RedirectToAction("EmployeeList");
        }

        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var result = await _employeeService.GetEmployee(id);
            if (result == null)
            {
                return RedirectToAction("EmployeeList");
            }
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditedEmployee(EmployeeModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEmployee", model);
            }

            var (isSuccess, result, errorMessage) = await _employeeService.SaveEditedEmployee(model, id);
            if (isSuccess)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View("EditEmployee", model);
            }
        }

        public async Task<IActionResult> CreateEmployee(EmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _employeeService.CreateEmployee(model);
            return RedirectToAction("EmployeeList");
        }
    }
}

