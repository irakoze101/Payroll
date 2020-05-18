using FluentAssertions;
using Payroll.Server.Mappings;
using Payroll.Server.Models;
using Payroll.Shared.DTO;
using Xunit;

namespace Payroll.Tests
{
    // TODO: a test to verify that the shape of Employee/EmployeeDto hasn't changed since the unit tests were written
    public class TestEmployeeMappings
    {
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
    }
}
