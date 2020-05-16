using System.Collections.Generic;

namespace Payroll.Shared.DTO
{
    public class EmployeeSummaryDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal GrossAnnualSalary { get; set; }
        public decimal BenefitsCostPerPay { get; set; }
        public decimal NetPaycheck { get; set; }
    }

    public class PayrollSummaryDto
    {
        public List<EmployeeSummaryDto> Employees { get; set; } = new List<EmployeeSummaryDto>();
    }
}
