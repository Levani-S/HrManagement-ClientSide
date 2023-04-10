using HRManagementClientSide.Models;
using HRManagementClientSide.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRManagementClientSide.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountsService _accountsService;
        public AccountsController(ILogger<AccountsController> logger,IAccountsService accountsService)
        {
            _logger = logger;
            _accountsService = accountsService;
        }

        public IActionResult RegistrationForm()
        {
            return View("~/Views/Accounts/RegistrationForm.cshtml");
        }
        public IActionResult LoginForm()
        {
            return View("~/Views/Accounts/LoginForm.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountsService.Register(model);
            if (result)
            {
                return RedirectToAction("GetAllEmployee", "Employee");
            }
            else
            {
                return RedirectToAction("RegistrationForm");
            }
        }
        [HttpPost]
        public async Task<IActionResult> LogInAsync(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountsService.LogInAsync(model);
            if (result)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

