using System.Collections.Generic;

namespace Payroll.Server.Benefits
{
    public interface IEmployee : IPerson
    {
        decimal AnnualSalary { get; }
        IEnumerable<IPerson> Dependents { get; }
    }
}
