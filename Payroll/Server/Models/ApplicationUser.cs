using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Payroll.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Of course in a real application these would be on an "Employer" table associated with the user.
        // Probably shouldn't even be touching this table.
        public int PayPeriodsPerYear { get; set; } = 26;
        public virtual ICollection<Employee> Employees { get; set; } = null!;
    }
}
