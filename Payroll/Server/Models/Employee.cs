using Payroll.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Server.Models
{
    public class Employee : Person
    {
        [Range(1, Constants.Validation.MaxSalary)]
        public decimal AnnualSalary { get; set; } = 52_000;

        public string EmployerId { get; set; } = string.Empty;
        public virtual ApplicationUser? Employer { get; set; }

        public virtual ICollection<Dependent>? Dependents { get; set; }
    }
}
