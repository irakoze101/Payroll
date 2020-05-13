using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
    public class Employee : Person
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Every Employee must have a salary")]
        [Range(1, double.PositiveInfinity)]
        public decimal AnnualSalary { get; set; }

        public Person? Spouse { get; set; }

        public IList<Person> Dependents { get; set; }

        public Employee(string name,
                        decimal annualSalary,
                        Person? spouse,
                        IList<Person> dependents) : base(name)
        {
            AnnualSalary = annualSalary;
            Spouse = spouse;
            Dependents = dependents;
        }
    }
}
