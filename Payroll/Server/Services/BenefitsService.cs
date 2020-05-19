using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Server.Services
{
    // UnitTestTodo
    public class BenefitsService : IBenefitsService
    {
        private static class Cost
        {
            public const decimal Employee = 1_000m;
            public const decimal Dependent = 500m;
        }

        /// <summary>
        /// Calculates total annual benefit cost for an employee, based on
        /// the employee and their benefitts. Requires <see cref="Employee.Dependents"/>
        /// to be populated.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public decimal AnnualBenefitCost(Employee employee)
        {
            if (employee.Dependents == null)
            {
                throw new InvalidOperationException("Dependents must be loaded to calculate benefits.");
            }

            // Name starting with 'A' gets a 10% discount
            Func<Person, decimal, decimal> costForPerson = (person, cost) =>
            {
                if (string.IsNullOrWhiteSpace(person.Name)) return cost;

                if (person.Name.ToUpper()[0] == 'A')
                {
                    return cost * 0.9m;
                }
                return cost;
            };

            var totalCost = costForPerson(employee, Cost.Employee);

            foreach (var dependent in employee.Dependents)
            {
                totalCost += costForPerson(dependent, Cost.Dependent);
            }

            return totalCost;

        }
        public PayrollSummaryDto PayrollSummary(int payPeriodsPerYear, IEnumerable<Employee> employees)
        {
            var summary = new PayrollSummaryDto();

            foreach (var employee in employees.OrderBy(e => e.Name))
            {
                var benefitsCostPerPay = AnnualBenefitCost(employee) / payPeriodsPerYear;
                var netPaycheck = employee.AnnualSalary / payPeriodsPerYear - benefitsCostPerPay;
                summary.Employees.Add(new EmployeeSummaryDto
                {
                    BenefitsCostPerPay = benefitsCostPerPay,
                    GrossAnnualSalary = employee.AnnualSalary,
                    Name = employee.Name,
                    NetPaycheck = netPaycheck,
                });
            }

            return summary;
        }
    }
}
