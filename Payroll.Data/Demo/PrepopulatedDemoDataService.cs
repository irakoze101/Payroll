using Payroll.Data.Models;
using System;
using System.Collections.Generic;

namespace Payroll.Data.Demo
{
    public class PrepopulatedDemoDataService : DemoDataService
    {
        private const decimal _salary = 26m * 2000;
        protected override List<Employee> _initialEmployeeData => new List<Employee>
        {
            new Employee("Aaron",
                         _salary,
                         null,
                         new List<Person>
                         {
                         })
            {
                Id = Guid.NewGuid(),
            },
            new Employee("Brenda",
                         _salary,
                         null,
                         new List<Person>
                         {
                         })
            {
                Id = Guid.NewGuid(),
            },
            // TODO: Make sure you handle negative paychecks somehow 😬
            new Employee("Stefan",
                         _salary,
                         new Person("Felix"),
                         new List<Person>
                         {
                             new Person("Quatloo"),
                             new Person("Quark"),
                             new Person("Honey"),
                             new Person("Zephram"),
                             new Person("Eek"),
                         })
            {
                Id = Guid.NewGuid(),
            },
            new Employee("Zaphod",
                         _salary,
                         null,
                         new List<Person>())
            {
                Id = Guid.NewGuid(),
            },
        };
    }
}
