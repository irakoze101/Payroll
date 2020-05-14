using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Models
{
    public class EmployeeBenefit
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;

        public int BenefitId { get; set; }
        public virtual Benefit Benefit { get; set; } = null!;
    }
}
