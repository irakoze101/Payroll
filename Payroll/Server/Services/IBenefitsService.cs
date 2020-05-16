using Payroll.Server.Models;

namespace Payroll.Server.Services
{
    public interface IBenefitsService
    {
        /// <summary>
        /// Calculates annual benefit cost for an employee. Requires an Employee with Dependents populated.
        /// </summary>
        decimal AnnualBenefitCost(Employee employee);
    }
}
