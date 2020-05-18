using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Repos
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string typeName, object id) : base($"No {typeName} found with id {id}")
        {
        }
    }
}
