using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Controllers
{
    public class PayrollControllerBase : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        protected PayrollControllerBase(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<string?> UserId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
