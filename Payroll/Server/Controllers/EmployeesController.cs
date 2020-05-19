using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Server.Mappings;
using Payroll.Server.Models;
using Payroll.Server.Repos;
using Payroll.Shared.DTO;

namespace Payroll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : PayrollControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeesController(IEmployeeRepository employeeRepo) : base()
        {
            _employeeRepo = employeeRepo;
        }


        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            var employees = await _employeeRepo.GetAll(userId, true, cancelToken);
            return Ok(employees.Select(e => e.ToDto()));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            var employee = await _employeeRepo.Get(id, userId, true, cancelToken);

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

            try
            {
                await _employeeRepo.Update(id, dto, userId, cancelToken);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (MappingException e)
            {
                return BadRequest(new StringContent(e.Message));
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDto employeeDto, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            employeeDto.Id = await _employeeRepo.Create(employeeDto, userId, cancelToken);

            return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.Id }, employeeDto);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id, CancellationToken cancelToken)
        {
            var userId = GetUserId();
            if (userId == null) return new UnauthorizedResult();

            try
            {
                await _employeeRepo.Delete(id, userId, cancelToken);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
