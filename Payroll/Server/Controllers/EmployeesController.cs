using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Server.Data;
using Payroll.Shared.Models;

namespace Payroll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : PayrollControllerBase
    {
        protected EmployeesController(ApplicationDbContext context,
                                      UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(CancellationToken cancelToken)
        {
            var userId = await UserId();
            if (userId == null) return new UnauthorizedResult();

            var employees =  await _context.Employees.Where(e => e.EmployerId == userId)
                                                     .Include(e => e.Dependents)
                                                     .ToListAsync(cancelToken);
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id, CancellationToken cancelToken)
        {
            var userId = await UserId();
            if (userId == null) return new UnauthorizedResult();

            var employee = await _context.Employees.Include(e => e.Dependents)
                                                   .FirstOrDefaultAsync(e => e.Employer.Id == userId &&
                                                                             e.Id == id,
                                                                        cancelToken);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee, CancellationToken cancelToken)
        {
            var userId = await UserId();
            if (userId == null) return new UnauthorizedResult();

            if (id != employee.Id)
            {
                return BadRequest();
            }
            if (!await _context.Employees.AnyAsync(e => e.EmployerId == userId && e.Id == id,
                                                   cancelToken))
            {
                return NotFound();
            }

            _context.Entry(employee).State = EntityState.Modified;

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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee, CancellationToken cancelToken)
        {
            var userId = await UserId();
            if (userId == null) return new UnauthorizedResult();

            employee.EmployerId = userId;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancelToken);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id, CancellationToken cancelToken)
        {
            var userId = await UserId();
            if (userId == null) return new UnauthorizedResult();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Employer.Id == userId &&
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
