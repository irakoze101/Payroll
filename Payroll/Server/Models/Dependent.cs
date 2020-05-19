using Payroll.Server.Benefits;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Payroll.Server.Models
{
    public enum Relationship
    {
        Spouse,
        Child,
    };

    public class Dependent : IPerson
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public Relationship Relationship { get; set; }

        public int EmployeeId { get; set; }
        [JsonIgnore]
        public virtual Employee? Employee { get; set; }
    }
}
