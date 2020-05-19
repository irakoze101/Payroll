using Payroll.Server.Models;
using Payroll.Shared;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Payroll.Tests
{
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

        public EmployeeSeed(int id = 0,
                            string name = "Placeholder",
                            decimal salary = 52_000,
                            int spouseId = 0,
                            string? spouseName = null,
                            IEnumerable<(int, string)>? children = null,
                            string employerId = "Employer")
        {
            Id = id;
            Name = name;
            Salary = salary;
            SpouseId = spouseId;
            SpouseName = spouseName;
            Children = children ?? Enumerable.Empty<(int, string)>();
            EmployerId = employerId;
        }


        public EmployeeSeed(EmployeeSeedParams seedParams)
        {
            Id = seedParams.HasId ? Utils.RandomId() : 0;
            Name = Utils.RandomName();
            Salary = Utils.RandomSalary();
            SpouseId = (seedParams.HasSpouse && seedParams.SpouseHasId) ? Utils.RandomId() : 0;
            SpouseName = seedParams.HasSpouse ? Utils.RandomName() : null;
            Children = seedParams.ChildrenWithIds.Select(c => (c ? Utils.RandomId() : 0, Utils.RandomName())).ToList();
            EmployerId = Utils.RandomString(20);
        }
    }

    public class EmployeeTestData : TheoryData<Employee, EmployeeDto>
    {
        public static (Employee, EmployeeDto) GenerateEmployee(EmployeeSeedParams seedParams)
        {
            var seed = new EmployeeSeed(seedParams);
            return GenerateEmployee(seed);
        }

        public static (Employee, EmployeeDto) GenerateEmployee(EmployeeSeed seed)
        {
            return (Model(seed), Dto(seed));
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

        /// <summary>
        /// Generates all possible sequences of children ID states for a specific number of children
        /// </summary>
        /// <param name="nChildren">The number of children</param>
        /// <returns>All possible orderings of ID states for the given number of children (e.g. for 2 children,
        /// returns { { false, false }, { false, true }, { true, false }, { true, true } })</returns>
        private static IEnumerable<IEnumerable<bool>> AllChildrens(int nChildren)
        {
            if (nChildren < 0) throw new ArgumentException(nameof(nChildren));
            if (nChildren == 0)
            {
                yield return Enumerable.Empty<bool>();
                yield break;
            }
            foreach (var subsequence in AllChildrens(nChildren - 1))
            {
                yield return subsequence.Concat(true.Yield());
                yield return subsequence.Concat(false.Yield());
            }
        }

        private static IEnumerable<IEnumerable<bool>> AllChildrensForRange(int maxChildren)
        {
            return Enumerable.Range(0, maxChildren + 1)
                             .SelectMany(i => AllChildrens(i));
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
                    foreach (var children in AllChildrensForRange(maxChildren))
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
