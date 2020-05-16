using System.Collections.Generic;

namespace Payroll.Shared.DTO
{
    public class DependentDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class EmployeeDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal AnnualSalary { get; set; } = 52_000;
        public DependentDto? Spouse { get; set; }
        public List<DependentDto> Children { get; set; } = new List<DependentDto>();
    }
}
