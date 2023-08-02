using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore;

public class WriteDataRepository<TContext> : IWriteableDataRepository, IDisposable
    where TContext : DbContext
{
    private readonly TContext _context;

    public WriteDataRepository(TContext context)
    {
        _context = context;
    }

    public T Add<T>(T model) where T : class
    {
        return _context.Add(model).Entity;
    }

    public async Task<T> AddAsync<T>(T model, CancellationToken cancellationToken = default(CancellationToken))
        where T : class
    {
        return (await _context.AddAsync(model, cancellationToken)).Entity;
    }

    public void Update<T>(T model) where T : class
    {
        _context.Update(model);
    }

    public void Delete<T>(T model) where T : class
    {
        _context.Remove(model);
    }

    public async Task<bool> DeleteAsync<T>(params object[] primaryKeys) where T : class
    {
        var model = await _context.FindAsync<T>(primaryKeys);
        if (model == null)
        {
            return false;
        }

        Delete(model);

        return true;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}