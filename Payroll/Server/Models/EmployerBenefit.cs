using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public class EmployerBenefit
    {
        public int Id {get;set;}

        public int EmployerId { get; set; }
        public virtual ApplicationUser Employer { get; set; } = null!;

        public int BenefitId { get; set; }
        public virtual Benefit Benefit { get; set; } = null!;
    }
}
