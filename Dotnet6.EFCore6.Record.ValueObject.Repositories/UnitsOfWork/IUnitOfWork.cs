using System.Threading;
using System.Threading.Tasks;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    }
}