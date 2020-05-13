using Payroll.Data.Models;
using Payroll.Data.Services;
using System;
using System.Collections.Generic;

namespace Payroll.Data.Demo
{
    /// <summary>
    /// Non-persistent data service that simply wraps a dictionary of IDs to Employees, starting empty.
    /// </summary>
    public class DemoDataService : IDataService
    {
        private readonly Dictionary<Guid, Employee> _employees = new Dictionary<Guid, Employee>();

        private static class Costs
        {
            public const decimal Employee = 1000m;
            public const decimal Spouse = 500m;
            public const decimal Dependent = 500m;
        }

        private readonly Benefit _benefit = new Benefit()
        {
            Name = "Benefits",
            CalculateCost = employee =>
            {
                Func<Person, decimal, decimal> costForPerson = (person, cost) =>
                {
                    if (string.IsNullOrWhiteSpace(person.Name)) return cost;

                    if (person.Name.ToUpper()[0] == 'A')
                    {
                        return cost * 0.9m;
                    }
                    return cost;
                };

                var totalCost = costForPerson(employee, Costs.Employee);

                if (employee.Spouse != null)
                {
                    totalCost += costForPerson(employee.Spouse, Costs.Spouse);
                }

                foreach (var dependent in employee.Dependents)
                {
                    totalCost += costForPerson(dependent, Costs.Dependent);
                }

                return totalCost;
            },
        };

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

        public IReadOnlyCollection<Benefit> GetBenefits()
        {
            return new List<Benefit> { _benefit };
        }

        public Employer GetEmployer() => new Employer(GetEmployees(),
                                                      26,
                                                      GetBenefits());
    }
}
