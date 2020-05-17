using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MoreLinq;
using Payroll.Server.Models;
using Payroll.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Server.Mappings
{
    // UnitTestTodo: Everything in this class
    public static class EmployeeMappings
    {
        /// <summary>
        /// Creates a new Employee from a DTO. Only suitable for creating a new Employee.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="employerId"></param>
        /// <returns></returns>
        public static Employee ToEmployee(this EmployeeDto dto, string employerId)
        {
            var dependents = dto.Children.Select(d => new Dependent { Name = d.Name, Relationship = Relationship.Child })
                                           .ToList();
            if (dto.Spouse != null)
            {
                dependents.Add(new Dependent { Name = dto.Spouse.Name, Relationship = Relationship.Spouse });
            }
            return new Employee
            {
                AnnualSalary = dto.AnnualSalary,
                Dependents = dependents,
                EmployerId = employerId,
                Id = dto.Id ?? default,
                Name = dto.Name,
            };
        }

        /// <summary>
        /// Requires <see cref="Employee.Dependents"/> to be included.
        /// </summary>
        public static EmployeeDto ToDto(this Employee employee)
        {
            var children = employee.Dependents!.Where(d => d.Relationship == Relationship.Child);
            var spouse = employee.Dependents?.FirstOrDefault(d => d.Relationship == Relationship.Spouse);
            return new EmployeeDto
            {
                AnnualSalary = employee.AnnualSalary,
                Children = children.Select(d => new DependentDto { Id = d.Id, Name = d.Name }).ToList(),
                Id = employee.Id,
                Name = employee.Name,
                Spouse = spouse == null ? null : new DependentDto { Id = spouse.Id, Name = spouse.Name },
            };
        }

        /// <summary>
        /// Maps the DTO onto the Employee model. Requires a model with <see cref="Employee.Dependents"/> loaded.
        /// Throws a <see cref="MappingException"/> on a mismatch between DTO and model.
        /// </summary>
        public static void MapForUpdate(EmployeeDto from, Employee to)
        {
            if (to.Dependents == null)
            {
                throw new InvalidOperationException("Dependents must be loaded.");
            }

            to.Name = from.Name;
            to.AnnualSalary = from.AnnualSalary;
            var newSpouse = to.Dependents.FirstOrDefault(d => d.Relationship == Relationship.Spouse);
            if (newSpouse == null && from.Spouse != null)
            {
                to.Dependents.Add(new Dependent { Name = from.Spouse.Name, Relationship = Relationship.Spouse });
            }
            else if (newSpouse != null && from.Spouse == null)
            {
                to.Dependents.Remove(newSpouse);
            }
            else if (newSpouse != null && from.Spouse != null)
            {
                if (from.Spouse.Id.HasValue &&
                    newSpouse.Id != from.Spouse.Id)
                {
                    throw new MappingException("Spouse IDs do not match.");
                }
                newSpouse.Name = from.Spouse.Name;
            }

            // Update the name for any child DTO with an ID, create a dependent
            // for any child without an ID, and delete any child from the Employee
            // with no corresponding DTO present.
            var newChildDtos = new List<DependentDto>();
            var updateChildDtos = new Dictionary<int, DependentDto>();
            foreach (var child in from.Children)
            {
                if (child.Id != null)
                {
                    updateChildDtos[child.Id.Value] = child;
                }
                else
                {
                    newChildDtos.Add(child);
                }
            }

            var (updateChildren, deleteChildren) = to.Dependents.Where(d => d.Relationship == Relationship.Child)
                                                                .Partition(d => updateChildDtos.ContainsKey(d.Id));

            var invalidChild = updateChildDtos.Keys.FirstOrDefault(key => !updateChildren.Any(c => c.Id == key));
            if (invalidChild != default)
            {
                throw new MappingException($"No child with key {invalidChild} for employee.");
            }

            foreach (var toChild in updateChildren)
            {
                toChild.Name = updateChildDtos[toChild.Id].Name;
            }
            foreach (var deleteChild in deleteChildren)
            {
                to.Dependents.Remove(deleteChild);
            }
            foreach (var newChildDto in newChildDtos)
            {
                to.Dependents.Add(new Dependent { Name = newChildDto.Name, Relationship = Relationship.Child });
            }
        }
    }
}
