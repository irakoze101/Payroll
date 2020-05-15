using Payroll.Shared.Models;

namespace Payroll.Shared.EditModels
{
    // UnitTestTodo
    public class PersonEditModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public PersonEditModel() { }

        public PersonEditModel(Person person)
        {
            Id = person.Id;
            Name = person.Name;
        }
    }
}
