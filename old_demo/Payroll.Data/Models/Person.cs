using System.ComponentModel.DataAnnotations;

namespace Payroll.Data.Models
{
    public class Person
    {
        // LocalizationTodo: all annotation error messages (see https://stackoverflow.com/questions/20699594/how-localize-errormessage-in-dataannotation )
        [Required(AllowEmptyStrings = false, ErrorMessage = "Everybody needs a name that isn't null or empty!")]
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }
    }
}
