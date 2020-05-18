using System.Threading;
using System.Threading.Tasks;

namespace Payroll.Server.Repos
{
    public interface IEmployerRepository
    {
        Task<int> GetPayPeriodsPerYear(string employerId, CancellationToken cancelToken);
    }
}