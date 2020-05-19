using Microsoft.EntityFrameworkCore;
using Payroll.Server.Data;
using Payroll.Server.Mappings;
using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payroll.Server.Repos
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Get(int id, string employerId, bool includeDependents, CancellationToken cancelToken)
        {
            var query = (IQueryable<Employee>)_context.Employees;
            if (includeDependents)
            {
                query = query.Include(e => e.Dependents);
            }
            var employee = await query.FirstOrDefaultAsync(e => e.Employer!.Id == employerId &&
                                                                e.Id == id,
                                                           cancelToken);
            if (employee == null)
            {
                throw new EntityNotFoundException(nameof(Employee), id);
            }
            return employee;
        }

        public Task<List<Employee>> GetAll(string employerId, bool includeDependents, CancellationToken cancelToken)
        {
            var query = (IQueryable<Employee>)_context.Employees;
            if (includeDependents)
            {
                query = query.Include(e => e.Dependents);
            }
            return query.Where(e => e.EmployerId == employerId)
                        .ToListAsync(cancelToken);
        }

        public async Task<int> Create(EmployeeDto dto, string employerId, CancellationToken cancelToken)
        {
            var employee = dto.ToEmployee(employerId);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancelToken);
            return employee.Id;
        }

        public async Task Update(int id, EmployeeDto dto, string employerId, CancellationToken cancelToken)
        {
            var employee = await _context.Employees.Include(e => e.Dependents)
                                                   .FirstOrDefaultAsync(e => e.EmployerId == employerId && e.Id == id,
                                                                        cancelToken);
            if (employee == null)
            {
                throw new EntityNotFoundException(nameof(Employee), id);
            }

            employee.UpdateFrom(dto);

            await _context.SaveChangesAsync(cancelToken);
        }

        public async Task Delete(int id, string employerId, CancellationToken cancelToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Employer!.Id == employerId &&
                                                                             e.Id == id,
                                                                        cancelToken);
            if (employee == null)
            {
                throw new EntityNotFoundException(nameof(Employee), id);
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancelToken);
        }
    }
}
