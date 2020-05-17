using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Shared.DTO
{
    public class DependentDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"[^\s]+", ErrorMessage = Constants.ErrorMessages.NonWhitespaceNameRequired)]
        public string Name { get; set; } = string.Empty;
    }

    public class EmployeeDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"[^\s]+", ErrorMessage = Constants.ErrorMessages.NonWhitespaceNameRequired)]
        public string Name { get; set; } = string.Empty;

        [Range(1, Constants.Validation.MaxSalary)]
        public decimal AnnualSalary { get; set; } = 52_000;
        public DependentDto? Spouse { get; set; }
        [Required]
        public List<DependentDto> Children { get; set; } = new List<DependentDto>();
    }
}
