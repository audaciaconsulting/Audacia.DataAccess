using System.Threading;
using System.Threading.Tasks;

namespace Audacia.DataAccess
{
    /// <summary>
    /// Exposes the methods to allow changes made to tracked instances of the model to be persisted to the underlying data storage mechanism.
    /// </summary>
    public interface ISaveableDataRepository
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}