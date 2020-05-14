using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
    public class Employer
    {
        public IReadOnlyCollection<Employee> Employees { get; set; }

        [Required(ErrorMessage = "Number of pay periods per year is required")]
        [Range(1, 365, ErrorMessage = "Number of pay periods must be between 1 and 365 inclusive")]
        public int PayPeriodsPerYear { get; set; }

        public IReadOnlyCollection<IBenefit> Benefits { get; set; }

        public Employer(IReadOnlyCollection<Employee> employees,
                        int payPeriodsPerYear,
                        IReadOnlyCollection<IBenefit> benefits)
        {
            Employees = employees;
            PayPeriodsPerYear = payPeriodsPerYear;
            Benefits = benefits;
        }
    }
}
