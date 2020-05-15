using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll.Server.Models;

namespace Payroll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : PayrollControllerBase
    {
        protected PayrollController(UserManager<ApplicationUser> userManager) : base(userManager)
        {
        }
    }
}
