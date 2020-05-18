using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using MoreLinq;
using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Payroll.Tests
{
    public static class PayrollAssert
    {
        public static void Equal(Dependent model, DependentDto dto)
        {
            Assert.Equal(model.Id, dto.Id ?? 0);
            Assert.Equal(model.Name, dto.Name);
        }

        public static void Equal(Employee model, EmployeeDto dto)
        {
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(model.Id, dto.Id ?? 0);
            Assert.Equal(model.AnnualSalary, dto.AnnualSalary);
            var (spouses, children) = model.Dependents.Partition(d => d.Relationship == Relationship.Spouse);
            var spouse = spouses.FirstOrDefault();
            if (spouse == null)
            {
                Assert.Null(dto.Spouse);
            }
            else
            {
                Equal(spouse, dto.Spouse!);
            }
            if (!children.Any())
            {
                Assert.Empty(dto.Children);
            }
            else
            {
                Assert.Equal(children.Count(), dto.Children.Count);
                foreach (var (modelChild, dtoChild) in children.OrderBy(c => c.Id)
                                                               .ThenBy(c => c.Name)
                                                               .Zip(dto.Children.OrderBy(c => c.Id)
                                                                                .ThenBy(c => c.Name),
                                                                    (m, d) => (m, d)))
                {
                    Equal(modelChild, dtoChild);
                }
            }
        }
    }
}
