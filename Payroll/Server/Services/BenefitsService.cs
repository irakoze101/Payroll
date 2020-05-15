using Payroll.Shared.Models;
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

        public decimal AnnualBenefitCost(Employee employee)
        {
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
