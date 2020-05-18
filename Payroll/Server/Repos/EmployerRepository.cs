using Microsoft.EntityFrameworkCore;
using Payroll.Server.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Payroll.Server.Repos
{
    public class EmployerRepository : RepositoryBase, IEmployerRepository
    {
        public EmployerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetPayPeriodsPerYear(string employerId, CancellationToken cancelToken)
        {
            var employer = await _context.Users.FirstOrDefaultAsync(u => u.Id == employerId, cancelToken);
            if (employer == null)
            {
                throw new EntityNotFoundException("Employer", employerId);
            }
            return employer.PayPeriodsPerYear;
        }
    }
}
