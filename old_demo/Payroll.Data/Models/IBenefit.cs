using System;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
    public interface IBenefit
    {
        public string Name { get; }

        /// <summary>
        /// Calculates the annual deduction for an employee.
        /// </summary>
        public decimal AnnualCost(Employee employee);
    }
}
