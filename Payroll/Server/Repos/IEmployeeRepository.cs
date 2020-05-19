using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Payroll.Server.Repos
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Creates a new Employee.
        /// </summary>
        Task<int> Create(EmployeeDto dto, string employerId, CancellationToken cancelToken);
        /// <summary>
        /// Gets an existing Employee.
        /// </summary>
        /// <remarks>Throws <see cref="EntityNotFoundException"/> if employee with given ID and EmployerId is not found.</remarks>
        Task<Employee> Get(int id, string employerId, bool includeDependents, CancellationToken cancelToken);
        Task<List<Employee>> GetAll(string employerId, bool includeDependents, CancellationToken cancelToken);
        /// <summary>
        /// Updates an existing Employee.
        /// </summary>
        /// <remarks>Throws <see cref="EntityNotFoundException"/> if employee with given ID and EmployerId is not found.
        /// Throws <see cref="Mappings.MappingException"/> if DTO-to-model mapping fails.</remarks>
        Task Update(int id, EmployeeDto dto, string employerId, CancellationToken cancelToken);
        /// <summary>
        /// Deletes an existing employee.
        /// </summary>
        /// <remarks>Throws <see cref="EntityNotFoundException"/> if employee with given ID and EmployerId is not found.
        Task Delete(int id, string employerId, CancellationToken cancelToken);
    }
}