using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Shared.ApiModels
{
    public class Employee
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Every Employee must have a salary")]
        [Range(1, double.PositiveInfinity)]
        public decimal AnnualSalary { get; set; }

        public string? Spouse { get; set; }

        public IList<string> Dependents { get; set; } = new List<string>();
    }
}
