namespace Payroll.Shared
{
    public static class Constants
    {
        public static class Validation
        {
            // Backing field is a decimal(18,2) (inflation-proof!)
            public const double MaxSalary = 9_999_999_999_999_999.99;
        }

        public static class ErrorMessages
        {
            public const string NonWhitespaceNameRequired = "A non-whitespace name is required.";
        }
    }
}
