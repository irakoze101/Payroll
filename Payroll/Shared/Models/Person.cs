using System.ComponentModel.DataAnnotations;

namespace Payroll.Shared.Models
{
    public abstract class Person
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
