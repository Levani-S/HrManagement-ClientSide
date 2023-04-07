using Microsoft.AspNetCore.Mvc;

namespace HRManagementClientSide.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }
    }
}
