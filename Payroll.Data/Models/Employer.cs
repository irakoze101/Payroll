using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class Employer
    {
        public IList<Employee> Employees { get; set; }

        [Required(ErrorMessage = "Number of pay periods per year is required")]
        [Range(1, 365, ErrorMessage = "Number of pay periods must be between 1 and 365 inclusive")]
        public int PayPeriodsPerYear { get; set; }

        public IList<Benefit> Benefits { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}
