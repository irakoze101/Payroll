using System.Text.Json.Serialization;

namespace Payroll.Server.Models
{
    public enum Relationship
    {
        Spouse,
        Child,
    };

    public class Dependent : Person
    {
        public Relationship Relationship { get; set; }

        public int EmployeeId { get; set; }
        [JsonIgnore]
        public virtual Employee? Employee { get; set; }
    }
}
