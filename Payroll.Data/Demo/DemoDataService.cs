using Payroll.Data.Models;
using Payroll.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Data.Demo
{
    /// <summary>
    /// Non-persistent data service that simply wraps a dictionary of IDs to Employees, starting empty.
    /// </summary>
    public class DemoDataService : IDataService
    {
        protected virtual List<Employee> _initialEmployeeData => new List<Employee>();
        protected readonly Dictionary<Guid, Employee> _employees;

        private readonly IBenefit _benefit = new DemoBenefit();

        public DemoDataService()
        {
            try
            {
                // Intentional dereference
#pragma warning disable CS8629 // Nullable value type may be null.
                _employees = _initialEmployeeData.ToDictionary(e => e.Id.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
            }
            catch (NullReferenceException e)
            {
                throw new InvalidOperationException("All Employees in initial data must have an ID!", e);
            }
        }

        public IReadOnlyCollection<Employee> GetEmployees()
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

        public IReadOnlyCollection<IBenefit> GetBenefits()
        {
            return new List<IBenefit> { _benefit };
        }

        public Employer GetEmployer() => new Employer(GetEmployees(),
                                                      26,
                                                      GetBenefits());
    }
}
