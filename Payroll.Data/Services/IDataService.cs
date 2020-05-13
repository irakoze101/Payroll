using Payroll.Models;
using System.Collections.Generic;

namespace Payroll.Services
{
    interface IDataService
    {
        /// <summary>
        /// Returns all employees
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<Employee> AllEmployees();
        /// <summary>
        /// If <see cref="Employee.Id"/> is null, creates a new employee record and updates the
        /// parameter's Id; otherwise, updates an existing record.
        /// </summary>
        /// <param name="employee">The employee to create or update</param>
        void SaveEmployee(Employee employee);
        /// <summary>
        /// Deletes an existing employee. <see cref="Employee.Id"/> must not be null.
        /// </summary>
        /// <param name="employee">The employee to delete</param>
        void DeleteEmployee(Employee employee);
    }
}
