using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Tests
{
    public static class TestExtensions
    {
        public static IEnumerable<T> Yield<T>(this T t)
        {
            yield return t;
        }
    }
}
