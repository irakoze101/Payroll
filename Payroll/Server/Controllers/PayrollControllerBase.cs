using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll.Server.Data;
using Payroll.Shared.Models;
using System.Threading.Tasks;

namespace Payroll.Server.Controllers
{
    public class PayrollControllerBase : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ApplicationDbContext _context;

        protected PayrollControllerBase(ApplicationDbContext context,
                                        UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        protected async Task<string?> UserId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
