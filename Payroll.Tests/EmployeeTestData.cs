using Payroll.Server.Models;
using Payroll.Shared;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Payroll.Tests
{
    public class EmployeeTestData : TheoryData<Employee, EmployeeDto>
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
            public int Id;
            public string Name;
            public decimal Salary;
            public int SpouseId;
            public string? SpouseName;
            public IEnumerable<(int, string)> Children;
            public string EmployerId;

            public EmployeeSeed(EmployeeSeedParams seedParams)
            {
                Id = seedParams.HasId ? RandomId() : 0;
                Name = RandomName();
                Salary = RandomSalary();
                SpouseId = (seedParams.HasSpouse && seedParams.SpouseHasId) ? RandomId() : 0;
                SpouseName = seedParams.HasSpouse ? RandomName() : null;
                Children = seedParams.ChildrenWithIds.Select(c => (c ? RandomId() : 0, RandomName())).ToList();
                EmployerId = RandomString(20);
            }
        }

        private static int RandomId() => _random.Next(1, int.MaxValue);

        private static string RandomString(int length)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var newString = new char[length];
            for (int i = 0; i < length; i++)
            {
                newString[i] = alphabet[_random.Next(26)];
            }
            return new string(newString);
        }

        private static string RandomName()
        {
            return $"{RandomString(5)} {RandomString(5)}";
        }

        private static decimal RandomSalary() => (decimal)(_random.NextDouble() * Constants.Validation.MaxSalary);

        public static (Employee, EmployeeDto) GenerateEmployee(EmployeeSeedParams seedParams)
        {
            var seed = new EmployeeSeed(seedParams);
            var model = Model(seed);
            var dto = Dto(seed);
            return (model, dto);
        }

        public static Employee Model(EmployeeSeed seed)
        {
            var model = new Employee
            {
                Id = seed.Id,
                Name = seed.Name,
                AnnualSalary = seed.Salary,
                Dependents = new List<Dependent>(),
                EmployerId = seed.EmployerId,
            };
            if (seed.SpouseName != null)
            {
                model.Dependents.Add(new Dependent { Id = seed.SpouseId, Name = seed.SpouseName, Relationship = Relationship.Spouse });
            }
            foreach (var (childId, childName) in seed.Children)
            {
                model.Dependents.Add(new Dependent { Id = childId, Name = childName, Relationship = Relationship.Child });
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
                Children = seed.Children.Select(((int childId, string childName) c) => new DependentDto { Id = c.childId, Name = c.childName }).ToList(),
            };
            return dto;
        }

        private static (Employee, EmployeeDto) EmployeeData(EmployeeSeed seed)
        {
            return (Model(seed), Dto(seed));
        }

        private static IEnumerable<IEnumerable<bool>> AllChildrens(int maxChildren)
        {
            if (maxChildren < 0) throw new ArgumentException(nameof(maxChildren));
            if (maxChildren == 0)
            {
                yield return Enumerable.Empty<bool>();
                yield break;
            }
            foreach (var subsequence in AllChildrens(maxChildren - 1))
            {
                yield return subsequence.Concat(true.Yield());
                yield return subsequence.Concat(false.Yield());
            }
        }

        /// <summary>
        /// Generates <see cref="EmployeeSeedParams"/> for all valid combinations
        /// of employee having ID, spouse existing, having ID, children existing
        /// and having IDs up to 3 children inclusive
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<EmployeeSeedParams> GenerateSeedParams()
        {
            var bools = new bool[2] { false, true };
            const int maxChildren = 3;
            // Employee without ID (thus no dependent IDs either
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
            foreach (var hasSpouse in bools)
            {
                foreach (var spouseHasId in bools)
                {
                    foreach (var children in AllChildrens(maxChildren))
                    {
                        yield return new EmployeeSeedParams
                        {
                            HasId = true,
                            HasSpouse = hasSpouse,
                            SpouseHasId = spouseHasId,
                            ChildrenWithIds = children,
                        };
                    }
                }
            }
        }

        private static Lazy<List<(Employee Model, EmployeeDto Dto)>> _allEmployees = new Lazy<List<(Employee, EmployeeDto)>>(() =>
        {
            return GenerateSeedParams().Select(p => GenerateEmployee(p)).ToList();
        });

        public EmployeeTestData()
        {
            foreach (var employee in _allEmployees.Value)
            {
                // TODO: Pretty sure I remember there being a smoother way to do this
                Add(employee.Model, employee.Dto);
            }
        }
    }
}
