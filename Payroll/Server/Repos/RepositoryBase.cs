using Payroll.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Server.Repos
{
    public class RepositoryBase
    {
        protected ApplicationDbContext _context;

        protected RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
