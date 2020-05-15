namespace Payroll.Shared.Models
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
        public virtual Employee Employee { get; set; } = null!;
    }
}
