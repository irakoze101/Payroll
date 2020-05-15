using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Shared.Models
{
    public class Employee : Person
    {
        public decimal AnnualSalary { get; set; } = 52_000;

        [Required]
        public string EmployerId { get; set; } = null!;
        public virtual ApplicationUser Employer { get; set; } = null!;

        public virtual ICollection<Dependent> Dependents { get; set; } = null!;
    }
}
