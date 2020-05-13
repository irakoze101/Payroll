﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class Benefit
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Deduction name cannot be null or empty")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A function that calculates the annual deduction for an employee.
        /// </summary>
        /// <remarks>Implementing this as a function property instead of as a method allows us to more
        /// dynamically generate benefit instances - could import definitions from a configuration file,
        /// for example</remarks>
        public Func<Employee, decimal> CalculateCost { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}
