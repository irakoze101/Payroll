using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Payroll.Server.Controllers
{
    public class PayrollControllerBase : ControllerBase
    {
        protected string? GetUserId()
        {
            // For the life of me, I can't get UserManager to work, so this will have to do.
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
