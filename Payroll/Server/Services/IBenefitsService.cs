using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System.Collections;
using System.Collections.Generic;

namespace Payroll.Server.Services
{
    public interface IBenefitsService
    {
        /// <summary>
        /// Calculates annual benefit cost for an employee. Requires an Employee with Dependents populated.
        /// </summary>
        decimal AnnualBenefitCost(Employee employee);
        /// <summary>
        /// Generates <see cref="PayrollSummaryDto"/> for the given employees. Requires Employees with Dependents populated.
        /// </summary>
        /// <param name="payPeriodsPerYear">The number of pay periods in a year.</param>
        /// <param name="employees">Employees to calculate benefit costs for.</param>
        /// <returns></returns>
        PayrollSummaryDto PayrollSummary(int payPeriodsPerYear, IEnumerable<Employee> employees);
    }
}
