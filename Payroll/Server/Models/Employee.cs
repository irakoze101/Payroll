using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Server.Models
{
    public class Employee : Person
    {
        public decimal AnnualSalary { get; set; } = 52_000;

        // TODO: Making EmployerId non-nullable makes POSTs/PUTs fail when EmployerId
        // is empty, but I don't want client to have to worry about this field.
        // Is initializing this to string.Empty the right approach?
        public string EmployerId { get; set; } = string.Empty;
        public virtual ApplicationUser? Employer { get; set; }

        public virtual ICollection<Dependent>? Dependents { get; set; }
    }
}
