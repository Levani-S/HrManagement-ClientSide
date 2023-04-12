using HRManagementClientSide.Models;
using HRManagementClientSide.Services;
using HRManagementClientSide.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementClientSide.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
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
                return View("RegistrationForm", model);
            }

            var result = await _accountsService.Register(model);
            if (result)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User already exists with that Username or E-mail please Log-In");
                return View("RegistrationForm", model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginForm", model);
            }
            var result = await _accountsService.LogIn(model);
            if (result)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            else
            {
                ModelState.AddModelError("", "User or Password is incorrect");
                return View("LoginForm", model);
            }
        }
        public IActionResult Logout()
        {
            _accountsService.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}


