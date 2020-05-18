using FluentAssertions;
using Payroll.Server.Mappings;
using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System.Collections.Generic;
using Xunit;

namespace Payroll.Tests
{
    // TODO: a test to verify that the shapes of Employee & EmployeeDto haven't changed since the unit tests were written
    public class TestEmployeeMappings
    {
        private static object[] UpdateCase(EmployeeSeed oldSeed, EmployeeSeed newSeed)
        {
            var oldModel = EmployeeTestData.Model(oldSeed);
            var (newModel, dto) = EmployeeTestData.GenerateEmployee(newSeed);
            return new object[] { oldModel, dto, newModel };
        }

        public static IEnumerable<object[]> MapForUpdateData => new List<object[]>
        {
            // Add a spouse
            UpdateCase(new EmployeeSeed(id: 3, name: "Cherry"),
                       new EmployeeSeed(id: 3, name: "Cherry", spouseName: "Audie")),
            // Add children
            UpdateCase(new EmployeeSeed(id: 5, name: "Tom"),
                       new EmployeeSeed(id: 5, name: "Tom", children: new List<(int, string)> { (0, "Timmy"), (0, "Tommy"), })),
            // Add one child when children exist
            UpdateCase(new EmployeeSeed(id: 10, name: "Homer", spouseId: 1, spouseName: "Marge", children: new List<(int, string)> { (2, "Bart"), (3, "Lisa") }),
                       new EmployeeSeed(id: 10, name: "Homer", spouseId: 1, spouseName: "Marge", children: new List<(int, string)> { (2, "Bart"), (3, "Lisa"), (0, "Maggie") })),
            // Remove a child
            UpdateCase(new EmployeeSeed(id: 10, name: "Homer", spouseId: 1, spouseName: "Marge", children: new List<(int, string)> { (2, "Bart"), (3, "Lisa"), (4, "Maggie"), (5, "Roy")}),
                       new EmployeeSeed(id: 10, name: "Homer", spouseId: 1, spouseName: "Marge", children: new List<(int, string)> { (2, "Bart"), (3, "Lisa"), (4, "Maggie") })),
            // Update spelling & salary on existing employee & dependents
            UpdateCase(new EmployeeSeed(id: 4,
                                        name: "oBb",
                                        salary: 50_000,
                                        spouseId: 1,
                                        spouseName: "Lnida",
                                        children: new List<(int, string)> { (2, "Tnia"), (3, "Gnee"), (4, "Luiose") }),
                       new EmployeeSeed(id: 4,
                                        name: "Bob",
                                        salary: 55_000,
                                        spouseId: 1,
                                        spouseName: "Linda",
                                        children: new List<(int, string)> { (2, "Tina"), (3, "Gene"), (4, "Louise") })),
        };


        [Theory]
        [ClassData(typeof(EmployeeTestData))]
        public void TestToDto(Employee model, EmployeeDto dto)
        {
            var dtoToTest = model.ToDto();
            dtoToTest.Should().BeEquivalentTo(dto);
        }

        [Theory]
        [ClassData(typeof(EmployeeTestData))]
        public void TestToModel(Employee model, EmployeeDto dto)
        {
            var modelToTest = dto.ToEmployee(model.EmployerId);
            modelToTest.Should().BeEquivalentTo(model);
        }

        [Theory]
        [MemberData(nameof(MapForUpdateData))]
        public void TestMapForUpdate(Employee oldModel, EmployeeDto dto, Employee expected)
        {
            oldModel.UpdateFrom(dto);
            oldModel.Should().BeEquivalentTo(expected);
        }
    }
}
