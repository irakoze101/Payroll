using Payroll.Data.Models;
using System;
using System.Collections.Generic;

namespace Payroll.Data.Demo
{
    public class PrepopulatedDemoDataService : DemoDataService
    {
        private const decimal _salary = 26m * 2_000;
        protected override List<Employee> _initialEmployeeData => new List<Employee>
        {
            new Employee("Aaron",
                         _salary,
                         null,
                         new List<Person>
                         {
                         })
            {
                Id = 1,
            },
            new Employee("Brenda",
                         _salary,
                         null,
                         new List<Person>
                         {
                         })
            {
                Id = 2,
            },
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
                Id = 3,
            },
            new Employee("Zaphod",
                         _salary,
                         null,
                         new List<Person>())
            {
                Id = 4
            },
        };
    }
}
