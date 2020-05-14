using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public enum BenefitType
    {
        [Description("Coding challenge benefit")]
        DemoBenefit,
    }

    public class Benefit
    {
        public int Id { get; set; }
        public BenefitType Type { get; set; }

        public virtual ICollection<EmployerBenefit> EmployerBenefits { get; set; } = null!;
        public virtual ICollection<EmployeeBenefit> Employee { get; set; } = null!;
    }
}
