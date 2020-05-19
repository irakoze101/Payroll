using FluentAssertions;
using Payroll.Server.Models;
using Payroll.Server.Services;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Payroll.Tests
{
    public class TestBenefitsService
    {
        public struct EmployeeCase
        {
            public readonly Employee Model;
            public readonly decimal BenefitCost;
            public readonly int PayPeriodsPerYear;
            public readonly decimal BenefitCostPerPeriod;
            public readonly decimal NetPayPerPeriod;

            public EmployeeSummaryDto Summary => new EmployeeSummaryDto
            {
                Name = Model.Name,
                BenefitsCostPerPay = BenefitCostPerPeriod,
                GrossAnnualSalary = Model.AnnualSalary,
                NetPaycheck = NetPayPerPeriod,
            };

            public EmployeeCase(EmployeeSeed seed,
                                decimal benefitCost,
                                int payPeriodsPerYear)
            {
                Model = EmployeeTestData.Model(seed);
                BenefitCost = benefitCost;
                PayPeriodsPerYear = payPeriodsPerYear;
                // If you're just repeating the same logic here that's
                // in the implementation, is this a good unit test?
                var grossPayPerPeriod = seed.Salary / PayPeriodsPerYear;
                BenefitCostPerPeriod = benefitCost / PayPeriodsPerYear;
                NetPayPerPeriod = grossPayPerPeriod - BenefitCostPerPeriod;
            }
        }

        private static EmployeeCase Akiko = new EmployeeCase(new EmployeeSeed(name: "Akiko",
                                                                              spouseName: "Ryuichi",
                                                                              salary: 50_000),
                                                             900 + 500,
                                                             24);

        private static EmployeeCase Haruomi = new EmployeeCase(new EmployeeSeed(name: "Haruomi",
                                                                                salary: 40_000),
                                                               1_000,
                                                               26);

        private static EmployeeCase Tatsuro = new EmployeeCase(new EmployeeSeed(name: "Tatsuro",
                                                                                spouseName: "Mariya",
                                                                                children: new List<(int, string)> { (0, "Chiemi") },
                                                                                salary: 30_000),
                                                               1_000 + 500 + 500,
                                                               52);

        private static EmployeeCase Toshiki = new EmployeeCase(new EmployeeSeed(name: "Toshiki",
                                                                                children: new List<(int, string)> { (0, "Anri") },
                                                                                salary: 60_000),
                                                               1_000 + 450,
                                                               4);

        private static EmployeeCase Masayoshi = new EmployeeCase(new EmployeeSeed(name: "Masayoshi",
                                                                                  children: new List<(int, string)> { (0, "Nobu") },
                                                                                  salary: 80_000),
                                                               1_000 + 500,
                                                               26);

        private static EmployeeCase Adam = new EmployeeCase(new EmployeeSeed(name: "Adam",
                                                                             salary: 100_000),
                                                            900,
                                                            26);

        public static List<EmployeeCase> AllEmployees = new List<EmployeeCase>
        {
            Akiko,
            Haruomi,
            Tatsuro,
            Toshiki,
            Masayoshi,
            Adam,
        };

        public static IEnumerable<object[]> AllEmployeesTheoryData => AllEmployees.Select(e => new object[] { e });
        /*
        public static TheoryData<EmployeeCase> AllEmployeesTheoryData = new TheoryData<EmployeeCase>
        {
            { Akiko },
            { Haruomi },
            { Tatsuro },
            { Toshiki },
            { Masayoshi },
            { Adam },
        };
        */


        [Theory]
        [MemberData(nameof(AllEmployeesTheoryData))]
        public void TestBenefitsCalculation(EmployeeCase @case)
        {
            var employee = @case.Model;
            var expectedCost = @case.BenefitCost;
            var benefitsService = new BenefitsService();
            var actualCost = benefitsService.AnnualBenefitCost(employee);
            Assert.Equal(expectedCost, actualCost);
        }

        // Verify that attempting to calculate benefits when Employee.Dependents
        // is not initialized will throw.
        [Fact]
        public void BenefitsThrowsOnNullDependents()
        {
            var benefitsService = new BenefitsService();
            var badEmployee = new Employee { Name = "Bob" };
            Assert.Throws<InvalidOperationException>(() => benefitsService.AnnualBenefitCost(badEmployee));
        }

        [Fact]
        public void PayrollSummaryIsSorted()
        {
            var expected = AllEmployees.OrderBy(e => e.Model.Name).Select(e => e.Model.Name).ToList();
            var benefitsService = new BenefitsService();
            var actual = benefitsService.PayrollSummary(26, AllEmployees.Select(e => e.Model)).Employees
                                        .Select(e => e.Name)
                                        .ToList();
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(AllEmployeesTheoryData))]
        public void PayrollSummaryIsAccurate(EmployeeCase @case)
        {
            var employees = new List<Employee> { @case.Model };
            var benefitsService = new BenefitsService();
            var summary = benefitsService.PayrollSummary(@case.PayPeriodsPerYear, employees);
            var employeeSummary = summary.Employees.First();
            employeeSummary.Should().BeEquivalentTo(@case.Summary);

        }
    }
}
