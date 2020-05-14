using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        public decimal AnnualSalary { get; set; } = 52_000;

        [Required]
        public string EmployerId { get; set; } = null!;
        public virtual ApplicationUser Employer { get; set; } = null!;

        public virtual ICollection<Dependent> Dependents { get; set; } = null!;
        public virtual ICollection<EmployeeBenefit> EmployeeBenefits { get; set; } = null!;
    }
}
