using FluentAssertions;
using Payroll.Server.Mappings;
using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Payroll.Tests
{
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
