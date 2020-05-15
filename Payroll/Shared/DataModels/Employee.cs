using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Payroll.Server.Models
{
    public class Employee : Person
    {
        public int Id { get; set; }

        public decimal AnnualSalary { get; set; } = 52_000;

        [Required]
        public string EmployerId { get; set; } = null!;
        public virtual ApplicationUser Employer { get; set; } = null!;

        public virtual ICollection<Dependent> Dependents { get; set; } = null!;
    }
}
