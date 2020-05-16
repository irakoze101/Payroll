using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Server.Data;
using Payroll.Server.Mappings;
using Payroll.Server.Models;
using Payroll.Shared.DTO;

namespace Payroll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : PayrollControllerBase
    {
        public EmployeesController(ApplicationDbContext context,
                                   UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            var employees =  await _context.Employees.Where(e => e.EmployerId == userId)
                                                     .Include(e => e.Dependents)
                                                     .ToListAsync(cancelToken);
            return Ok(employees.Select(e => e.ToDto()));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            var employee = await _context.Employees.Include(e => e.Dependents)
                                                   .FirstOrDefaultAsync(e => e.Employer!.Id == userId &&
                                                                             e.Id == id,
                                                                        cancelToken);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee.ToDto());
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDto dto, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            if (id != dto.Id)
            {
                return BadRequest();
            }

            var employee = await _context.Employees.Include(e => e.Dependents)
                                                   .FirstOrDefaultAsync(e => e.EmployerId == userId && e.Id == id,
                                                                        cancelToken);

            if (employee == null)
            {
                return NotFound();
            }

            try
            {
                EmployeeMappings.MapForUpdate(dto, employee);
            }
            catch (MappingException e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                await _context.SaveChangesAsync(cancelToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await EmployeeExistsAsync(id, cancelToken)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDto employeeDto, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            var employee = employeeDto.ToEmployee(userId);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancelToken);

            return CreatedAtAction("GetEmployee", new { id = employeeDto.Id }, employeeDto);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Employer!.Id == userId &&
                                                                             e.Id == id,
                                                                        cancelToken);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancelToken);

            return employee;
        }

        private Task<bool> EmployeeExistsAsync(int id, CancellationToken cancelToken = default)
        {
            return _context.Employees.AnyAsync(e => e.Id == id, cancelToken);
        }
    }
}
