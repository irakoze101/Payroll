using Payroll.Data.Models;
using Payroll.Data.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Payroll.Data.Demo
{
    /// <summary>
    /// Non-persistent data service that simply wraps a dictionary of IDs to Employees, starting empty.
    /// </summary>
    public class DemoDataService : IDataService
    {
        private readonly Dictionary<Guid, Employee> _employees = new Dictionary<Guid, Employee>();

        public IReadOnlyCollection<Employee> AllEmployees()
        {
            return _employees.Values;
        }

        public void DeleteEmployee(Employee employee)
        {
            var id = employee.Id ?? throw new InvalidOperationException("Employee with null Id cannot be deleted");
            if (!_employees.ContainsKey(id)) throw new InvalidOperationException($"Employee with Id {employee.Id} does not exist");
            _employees.Remove(id);
        }

        public void SaveEmployee(Employee employee)
        {
            if (!employee.Id.HasValue)
            {
                var newId = Guid.NewGuid();
                while (true)
                {
                    if (!_employees.ContainsKey(newId)) break;
                    newId = Guid.NewGuid();
                }
                employee.Id = newId;
            }

            _employees[employee.Id.Value] = employee;
        }
    }
}
