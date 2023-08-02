using System.Threading;
using System.Threading.Tasks;

namespace Audacia.DataAccess;

/// <summary>
/// Exposes the methods to add and remove model instances from the underlying data storage.
/// </summary>
public interface IWriteableDataRepository
{
    /// <summary>
    /// Adds the given <paramref name="model"/> to the data repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    T Add<T>(T model) where T : class;

    /// <summary>
    /// Adds the given <paramref name="model"/> to the data repository asynchronously
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<T> AddAsync<T>(T model, CancellationToken cancellationToken = default(CancellationToken)) where T : class;

    /// <summary>
    /// Updates the given <paramref name="model"/> in the data repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    void Update<T>(T model) where T : class;

    /// <summary>
    /// Deletes the given <paramref name="model"/> from the data repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    void Delete<T>(T model) where T : class;

    /// <summary>
    /// Deletes the model instance identified by the given <paramref name="primaryKeys"/> from the data repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="primaryKeys"></param>
    /// <returns>A <see cref="Task{TResult}"/> that, when complete, contains a <see cref="bool"/> value describing whether or not the deletion was successful.</returns>
    Task<bool> DeleteAsync<T>(params object[] primaryKeys) where T : class;
}