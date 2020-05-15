using Payroll.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
