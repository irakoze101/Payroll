using Payroll.Server.Benefits;
using Payroll.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Server.Models
{
    public class Employee : IEmployee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Range(1, Constants.Validation.MaxSalary)]
        public decimal AnnualSalary { get; set; } = 52_000;

        public string EmployerId { get; set; } = string.Empty;
        public virtual ApplicationUser? Employer { get; set; }

        public virtual ICollection<Dependent>? Dependents { get; set; }

        IEnumerable<IPerson> IEmployee.Dependents => Dependents ?? throw new InvalidOperationException("Dependents are not loaded");
    }
}
