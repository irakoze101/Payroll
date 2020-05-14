using Payroll.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Data.Demo
{
    public class DemoBenefit : IBenefit
    {
        private static class Costs
        {
            public const decimal Employee = 1000m;
            public const decimal Spouse = 500m;
            public const decimal Dependent = 500m;
        }

        public string Name => "Demo Benefit";

        public decimal AnnualCost(Employee employee)
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
        }
    }
}
