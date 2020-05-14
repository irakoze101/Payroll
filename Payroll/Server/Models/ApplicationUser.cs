using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int PayPeriodsPerYear { get; set; } = 26;
        public virtual ICollection<Employee> Employees { get; set; } = null!;
        public virtual ICollection<EmployerBenefit> EmployerBenefits { get; set; } = null!;
    }
}
