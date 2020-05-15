using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Server.Data;
using Payroll.Shared.Models;
using Payroll.Server.Services;
using Payroll.Shared.ApiModels;
using Microsoft.AspNetCore.Authorization;

namespace Payroll.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : PayrollControllerBase
    {
        private readonly IBenefitsService _benefitsService;

        protected PayrollController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    IBenefitsService benefitsService) : base(context, userManager)
        {
            _benefitsService = benefitsService;
        }

        [HttpGet]
        public async Task<ActionResult<PayrollSummary>> Summary(CancellationToken cancelToken)
        {
            var employer = await _userManager.GetUserAsync(User);
            if (employer == null) return new UnauthorizedResult();

            var employees = await _context.Employees.Include(e => e.Dependents)
                                                    .Where(e => e.EmployerId == employer.Id)
                                                    .ToListAsync(cancelToken);


            var summary = new PayrollSummary();

            foreach (var employee in employees)
            {
                var benefitsCostPerPay = _benefitsService.AnnualBenefitCost(employee) / employer.PayPeriodsPerYear;
                var netPaycheck = employee.AnnualSalary / employer.PayPeriodsPerYear - benefitsCostPerPay;
                summary.Employees.Add(new EmployeePayrollSummary
                {
                    BenefitsCostPerPay = benefitsCostPerPay,
                    GrossAnnualSalary = employee.AnnualSalary,
                    Name = employee.Name,
                    NetPaycheck = netPaycheck,
                });
            }

            return Ok(summary);
        }
    }
}
