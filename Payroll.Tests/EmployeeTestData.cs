using Payroll.Server.Models;
using Payroll.Shared;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Tests
{
    public static class EmployeeTestData
    {
        // Using the same seed every time allows for consistent test data... until the test cases
        // are changed. Should find a way to use a unique Random instance for each initialization.
        private static Random _random = new Random(42);

        public struct EmployeeSeedParams
        {
            public bool HasId;
            public bool HasSpouse;
            public bool SpouseHasId;
            public IEnumerable<bool> ChildrenWithIds;
        }

        public struct EmployeeSeed
        {
            public int? Id;
            public string Name;
            public decimal Salary;
            public int? SpouseId;
            public string? SpouseName;
            public IEnumerable<(int?, string)> Children;

            public EmployeeSeed(EmployeeSeedParams seedParams)
            {
                Id = seedParams.HasId ? RandomId() : (int?)null;
                Name = RandomName();
                Salary = RandomSalary();
                SpouseId = (seedParams.HasSpouse && seedParams.SpouseHasId) ? RandomId() : (int?)null;
                SpouseName = seedParams.HasSpouse ? RandomName() : null;
                Children = seedParams.ChildrenWithIds.Select(c => (c ? RandomId() : (int?)null, RandomName())).ToList();
            }
        }

        private static int RandomId() => _random.Next(1, int.MaxValue);

        private static string RandomName()
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var name = new char[11];
            for (int i = 0; i < 5; i++)
            {
                name[i] = alphabet[_random.Next(26)];
            }
            name[5] = ' ';
            for (int i = 6; i < 11; i++)
            {
                name[i] = alphabet[_random.Next(26)];
            }
            return new string(name);
        }

        private static decimal RandomSalary() => (decimal)(_random.NextDouble() * Constants.Validation.MaxSalary);

        public static (Employee, EmployeeDto) GenerateEmployee(EmployeeSeedParams seedParams)
        {
            var model = Model(new EmployeeSeed(seedParams));
            var dto = Dto(new EmployeeSeed(seedParams));
            return (model, dto);
        }

        public static Employee Model(EmployeeSeed seed)
        {
            var model = new Employee
            {
                Id = seed.Id ?? 0,
                Name = seed.Name,
                AnnualSalary = seed.Salary,
                Dependents = new List<Dependent>(),
            };
            if (seed.SpouseName != null)
            {
                model.Dependents.Add(new Dependent { Id = seed.SpouseId ?? 0, Name = seed.SpouseName, Relationship = Relationship.Spouse });
            }
            foreach (var (childId, childName) in seed.Children)
            {
                model.Dependents.Add(new Dependent { Id = childId ?? 0, Name = childName, Relationship = Relationship.Child });
            }

            return model;
        }

        public static EmployeeDto Dto(EmployeeSeed seed)
        {
            var dto = new EmployeeDto
            {
                Id = seed.Id,
                AnnualSalary = seed.Salary,
                Name = seed.Name,
                Spouse = seed.SpouseName == null ? null : new DependentDto { Id = seed.SpouseId, Name = seed.SpouseName },
                // This has been open for 3 years ☹ https://github.com/dotnet/csharplang/issues/258
                Children = seed.Children.Select(((int? childId, string childName) c) => new DependentDto { Id = c.childId, Name = c.childName }).ToList(),
            };
            return dto;
        }

        private static (Employee, EmployeeDto) EmployeeData(EmployeeSeed seed)
        {
            return (Model(seed), Dto(seed));
        }

        public static IEnumerable<EmployeeSeedParams> GenerateSeeds()
        {
            // An employee without an ID can't have dependents with IDs
            var bools = new bool[2] { false, true };
            const int maxChildren = 3;
            foreach (var hasSpouse in bools)
            {
                foreach (var nChildren in Enumerable.Range(0, maxChildren + 1))
                {
                    yield return new EmployeeSeedParams
                    {
                        HasId = false,
                        HasSpouse = hasSpouse,
                        SpouseHasId = false,
                        ChildrenWithIds = Enumerable.Repeat(false, nChildren),
                    };
                }
            }
            // TODO: employee with IDs
        }
    }
}
