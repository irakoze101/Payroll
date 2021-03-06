using Payroll.Data.Models;
using System.Collections.Generic;

namespace Payroll.Data.Services
{
    /// <summary>
    /// Service used to store and retrieve data models.
    /// </summary>
    /// <remarks>In an app that persisted data to a backend, this would have to be expanded to
    /// support multiple employers and only return data for the currently signed-in employer.
    /// </remarks>
    public interface IDataService
    {
        /// <summary>
        /// Gets the current employer.
        /// </summary>
        Employer GetEmployer();
        /// <summary>
        /// Gets employee by ID.
        /// </summary>
        Employee GetEmployee(int id);
        /// <summary>
        /// Returns all employees.
        /// </summary>
        IReadOnlyCollection<Employee> GetEmployees();
        /// <summary>
        /// If <see cref="Employee.Id"/> is null, creates a new employee record and updates the
        /// parameter's Id; otherwise, updates an existing record.
        /// </summary>
        void SaveEmployee(Employee employee);
        /// <summary>
        /// Deletes an existing employee. <see cref="Employee.Id"/> must not be null.
        /// </summary>
        void DeleteEmployee(Employee employee);
        /// <summary>
        /// Returns all benefits provided by the employer.
        /// </summary>
        IReadOnlyCollection<IBenefit> GetBenefits();
    }
}
