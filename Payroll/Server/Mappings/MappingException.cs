using System;

namespace Payroll.Server.Mappings
{
    public class MappingException : Exception
    {
        public MappingException(string message) : base(message)
        {
        }
    }
}
