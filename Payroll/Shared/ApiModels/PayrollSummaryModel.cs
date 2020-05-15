using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Shared.ApiModels
{
    public class EmployeePayrollSummary
    {
        public string Name { get; set; } = string.Empty;
        public decimal GrossAnnualSalary { get; set; }
        public decimal BenefitsCostPerPay { get; set; }
        public decimal NetPaycheck { get; set; }
    }

    public class PayrollSummary
    {
        public List<EmployeePayrollSummary> Employees { get; set; } = new List<EmployeePayrollSummary>();
    }
}
