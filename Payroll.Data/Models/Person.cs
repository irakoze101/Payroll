using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class Person
    {
        // LocalizationTodo: all annotation error messages (see https://stackoverflow.com/questions/20699594/how-localize-errormessage-in-dataannotation )
        [Required(AllowEmptyStrings = false, ErrorMessage = "Everybody needs a name that isn't null or empty!")]
        public string Name { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}
