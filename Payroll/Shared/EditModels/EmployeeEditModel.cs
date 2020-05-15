using Payroll.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Shared.EditModels
{
    // UnitTestTodo
    public class EmployeeEditModel : PersonEditModel
    {
        public decimal AnnualSalary { get; set; }
        public PersonEditModel? Spouse { get; set; } = new PersonEditModel();
        public List<PersonEditModel> Dependents { get; set; } = new List<PersonEditModel>();

        public EmployeeEditModel() { }

        public EmployeeEditModel(Employee employee) : base(employee)
        {
            AnnualSalary = employee.AnnualSalary;
            var spouse = employee.Dependents.SingleOrDefault(d => d.Relationship == Relationship.Spouse);
            if (spouse != null)
            {
                Spouse = new PersonEditModel(spouse);
            }
            foreach (var dependent in employee.Dependents.Where(d => d.Relationship != Relationship.Spouse))
            {
                Dependents.Add(new PersonEditModel(dependent));
            }
        }

        public Employee ToEmployee()
        {
            var employee = new Employee();
            employee.Id = Id ?? default;
            employee.Name = Name;
            employee.AnnualSalary = AnnualSalary;
            if (Spouse != null)
            {
                employee.Dependents.Add(new Dependent
                {
                    Id = Spouse.Id ?? default,
                    Name = Spouse.Name,
                    Relationship = Relationship.Spouse,
                });
            }
            foreach (var dependent in Dependents)
            {
                employee.Dependents.Add(new Dependent
                {
                    Id = dependent.Id ?? default,
                    Name = dependent.Name,
                    Relationship = Relationship.Child,
                });
            }
            return employee;
        }
    }
}
