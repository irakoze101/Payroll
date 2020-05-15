using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public abstract class Person
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
