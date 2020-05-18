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
            var dependents = dto.Children.Select(d => new Dependent
            {
                Id = d.Id,
                Name = d.Name,
                Relationship = Relationship.Child,
            })
                                           .ToList();
            if (dto.Spouse != null)
            {
                dependents.Add(new Dependent
                {
                    Id = dto.Spouse.Id,
                    Name = dto.Spouse.Name,
                    Relationship = Relationship.Spouse,
                });
            }
            return new Employee
            {
                AnnualSalary = dto.AnnualSalary,
                Dependents = dependents,
                EmployerId = employerId,
                Id = dto.Id,
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
                Children = children.Select(d => new DependentDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList(),
                Id = employee.Id,
                Name = employee.Name,
                Spouse = spouse == null ? null : new DependentDto
                {
                    Id = spouse.Id,
                    Name = spouse.Name
                },
            };
        }

        /// <summary>
        /// Maps the DTO onto the Employee model. Requires a model with <see cref="Employee.Dependents"/> loaded.
        /// Throws a <see cref="MappingException"/> on a mismatch between DTO and model.
        /// </summary>
        public static void UpdateFrom(this Employee to, EmployeeDto from)
        {
            if (to.Dependents == null)
            {
                throw new InvalidOperationException("Dependents must be loaded.");
            }
            if (to.Id != from.Id)
            {
                throw new MappingException($"IDs do not match (from: {from.Id}, to: {to.Id})");
            }

            to.Name = from.Name;
            to.AnnualSalary = from.AnnualSalary;

            var toSpouse = to.Dependents.FirstOrDefault(d => d.Relationship == Relationship.Spouse);
            if (toSpouse != null)
            {
                switch (from.Spouse?.Id)
                {
                    case 0:
                        to.Dependents.Add(new Dependent { Name = from.Spouse.Name, Relationship = Relationship.Spouse });
                        // This is the first goto I've ever written in C#, but what I really wanted was a fallthrough
                        goto case null;
                    case null:
                        to.Dependents.Remove(toSpouse);
                        break;
                    case var existingId when existingId == toSpouse.Id:
                        toSpouse.Name = from.Spouse.Name;
                        break;
                    default:
                        throw new MappingException("Spouse IDs do not match.");
                }
            }
            else if (from.Spouse != null)
            {
                if (from.Spouse.Id != 0)
                {
                    // Can only update existing spouse or create new one
                    throw new MappingException($"Spouse with ID {from.Spouse.Id} does not exist.");
                }
                to.Dependents.Add(new Dependent { Name = from.Spouse.Name, Relationship = Relationship.Spouse });
            }
            // else from and to spouses both null, nothing to do

            // Update the name for any child DTO with an ID, create a dependent
            // for any child without an ID, and delete any child from the Employee
            // with no corresponding DTO present.
            var newChildDtos = new List<DependentDto>();
            var updateChildDtos = new Dictionary<int, DependentDto>();
            foreach (var child in from.Children)
            {
                if (child.Id != 0)
                {
                    updateChildDtos[child.Id] = child;
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
