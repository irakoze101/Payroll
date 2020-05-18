using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Server.Data;
using Payroll.Server.Models;
using Payroll.Server.Services;
using Payroll.Shared.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Payroll.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : PayrollControllerBase
    {
        private readonly IBenefitsService _benefitsService;

        public PayrollController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 IBenefitsService benefitsService) : base(context, userManager)
        {
            _benefitsService = benefitsService;
        }

        [HttpGet("Summary")]
        public async Task<ActionResult<PayrollSummaryDto>> Summary(CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (string.IsNullOrWhiteSpace(userId)) return new UnauthorizedResult();

            var employer = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancelToken);
            if (employer == null)
            {
                return new UnauthorizedResult();
            }

            var employees = await _context.Employees.Include(e => e.Dependents)
                                                    .Where(e => e.EmployerId == employer.Id)
                                                    .OrderBy(e => e.Name)
                                                    .ToListAsync(cancelToken);


            var summary = new PayrollSummaryDto();

            foreach (var employee in employees)
            {
                var benefitsCostPerPay = _benefitsService.AnnualBenefitCost(employee) / employer.PayPeriodsPerYear;
                var netPaycheck = employee.AnnualSalary / employer.PayPeriodsPerYear - benefitsCostPerPay;
                summary.Employees.Add(new EmployeeSummaryDto
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
