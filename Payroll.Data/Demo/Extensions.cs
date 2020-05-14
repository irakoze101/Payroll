using Payroll.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Payroll.Data.Demo
{
    internal static class Extensions
    {
        // Demo data service needs to provide copies of the backing data.

        /// <summary>
        /// Creates a deep copy of the Employee.
        public static Employee Copy(this Employee employee)
        {
            return new Employee(employee.Name,
                                employee.AnnualSalary,
                                employee.Spouse != null ? new Person(employee.Spouse.Name) : null,
                                employee.Dependents.Select(d => new Person(d.Name)).ToList())
            {
                Id = employee.Id,
            };
        }

        /// <summary>
        /// Creates a deep copy of the Employer.
        /// </summary>
        public static Employer Copy(this Employer employer)
        {
            return new Employer(employer.Employees.Select(e => e.Copy()).ToList(),
                                employer.PayPeriodsPerYear,
                                employer.Benefits);
        }

        public static IBenefit Copy(this IBenefit benefit)
        {
            return benefit switch {
                DemoBenefit demoBenefit => new DemoBenefit(),
                _ => throw new NotImplementedException($"{nameof(Copy)} not implemented for benefit {benefit.Name}")
            };
        }
    }
}
