using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Server.Services;
using Payroll.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Payroll.Server.Repos;

namespace Payroll.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : PayrollControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IEmployerRepository _employerRepo;
        private readonly IBenefitsService _benefitsService;

        public PayrollController(IEmployeeRepository employeeRepository,
                                 IEmployerRepository employerRepository,
                                 IBenefitsService benefitsService) : base()
        {
            _employeeRepo = employeeRepository;
            _employerRepo = employerRepository;
            _benefitsService = benefitsService;
        }

        [HttpGet("Summary")]
        public async Task<ActionResult<PayrollSummaryDto>> Summary(CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (string.IsNullOrWhiteSpace(userId)) return new UnauthorizedResult();

            var payPeriodsPerYear = await _employerRepo.GetPayPeriodsPerYear(userId, cancelToken);
            var employees = await _employeeRepo.GetAll(userId, true, cancelToken);
            return Ok(_benefitsService.PayrollSummary(payPeriodsPerYear, employees));
        }
    }
}
