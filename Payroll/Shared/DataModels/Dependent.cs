using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public enum Relationship
    {
        Spouse,
        Child,
    };

    public class Dependent : Person
    {
        public int Id { get; set; }
        public Relationship Relationship { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
    }
}
