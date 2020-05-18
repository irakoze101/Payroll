using System;
using Payroll.Shared;

namespace Payroll.Tests
{
    public static class Utils
    {
        // Using the same seed every time allows for consistent test data... until the test cases
        // are changed. Should find a way to use a unique Random instance for each initialization.
        private static Random _random = new Random(42);
        public static int RandomId() => _random.Next(1, int.MaxValue);

        public static string RandomString(int length)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var newString = new char[length];
            for (int i = 0; i < length; i++)
            {
                newString[i] = alphabet[_random.Next(26)];
            }
            return new string(newString);
        }

        public static string RandomName()
        {
            return $"{RandomString(5)} {RandomString(5)}";
        }

        public static decimal RandomSalary() => (decimal)(_random.NextDouble() * Constants.Validation.MaxSalary);
    }
}
