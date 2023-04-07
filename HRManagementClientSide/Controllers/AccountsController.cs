using Microsoft.AspNetCore.Mvc;

namespace HRManagementClientSide.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }
        public IActionResult RegistrationForm()
        {
            return View("~/Views/Accounts/RegistrationForm.cshtml");
        }

    }
}
