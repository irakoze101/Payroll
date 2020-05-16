using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll.Server.Data;
using Payroll.Server.Models;
using System.Linq;
using System.Security.Claims;

namespace Payroll.Server.Controllers
{
    public class PayrollControllerBase : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ApplicationDbContext _context;

        protected PayrollControllerBase(ApplicationDbContext context,
                                        UserManager<ApplicationUser> userManager) : base()
        {
            _context = context;
            _userManager = userManager;
        }

        protected string? GetUserId()
        {
            // For the life of me, I can't get UserManager to work, so this will have to do.
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
