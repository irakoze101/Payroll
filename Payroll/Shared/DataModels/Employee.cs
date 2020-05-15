using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public Shared.ApiModels.Employee ToModel() => new Shared.ApiModels.Employee
        {
            Id = Id,
            AnnualSalary = AnnualSalary,
            Spouse = Dependents.FirstOrDefault(d => d.Relationship == Relationship.Spouse)?.Name,
            Dependents = Dependents.Where(d => d.Relationship == Relationship.Child)
                                   .Select(d => d.Name)
                                   .ToList(),
        };
    }
}
