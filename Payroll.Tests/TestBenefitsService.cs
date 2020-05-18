using Payroll.Server.Models;
using Payroll.Server.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Payroll.Tests
{
    public class TestBenefitsService
    {
        private class TestData : TheoryData<Employee, decimal>
        {
            public TestData()
            {
                Add(new Employee
                {
                    Name = "Akiko",
                    Dependents = new List<Dependent>
                    {
                        new Dependent
                        {
                            Name = "Ryuichi",
                            Relationship = Relationship.Spouse,
                        },
                    },
                }, 900 + 500);
                Add(new Employee
                {
                    Name = "Haruomi",
                    Dependents = new List<Dependent>(),
                }, 1000);
                Add(new Employee
                {
                    Name = "Tatsuro",
                    Dependents = new List<Dependent>
                    {
                        new Dependent
                        {
                            Name = "Mariya",
                            Relationship = Relationship.Spouse,
                        },
                        new Dependent
                        {
                            Name = "Chiemi",
                            Relationship = Relationship.Child,
                        },
                    },
                }, 1000 + 500 * 2);
                Add(new Employee
                {
                    Name = "Toshiki",
                    Dependents = new List<Dependent>
                    {
                        new Dependent
                        {
                            Name = "Anri",
                            Relationship = Relationship.Child,
                        },
                    }
                }, 1000 + 450);
                Add(new Employee
                {
                    Name = "Masayoshi",
                    Dependents = new List<Dependent>
                    {
                        new Dependent
                        {
                            Name = "Nobu",
                            Relationship = Relationship.Child,
                        }
                    }
                }, 1000 + 500);
                Add(new Employee
                {
                    Name = "Adam",
                    Dependents = new List<Dependent>(),
                }, 900);
            }
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void TestBenefitsCalculation(Employee employee, decimal expectedCost)
        {
            var benefitsService = new BenefitsService();
            var actualCost = benefitsService.AnnualBenefitCost(employee);
            Assert.Equal(expectedCost, actualCost);
        }

        // Verify that attempting to calculate benefits when Employee.Dependents
        // is not initialized will throw.
        [Fact]
        public void BenefitsThrowOnNullDependents()
        {
            var benefitsService = new BenefitsService();
            var badEmployee = new Employee { Name = "Bob" };
            Assert.Throws<InvalidOperationException>(() => benefitsService.AnnualBenefitCost(badEmployee));
        }
    }
}
