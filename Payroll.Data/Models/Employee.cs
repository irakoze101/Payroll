using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class Employee : Person
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Every Employee must have a salary")]
        [Range(1, double.PositiveInfinity)]
        public decimal AnnualSalary { get; set; }

        public Person? Spouse { get; set; }

        public IList<Person> Dependents { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}
