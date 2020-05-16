using Payroll.Server.Models;
using System;

namespace Payroll.Server.Services
{
    // UnitTestTodo
    public class BenefitsService : IBenefitsService
    {
        private static class Cost
        {
            public const decimal Employee = 1_000m;
            public const decimal Dependent = 5000m;
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
    }
}
