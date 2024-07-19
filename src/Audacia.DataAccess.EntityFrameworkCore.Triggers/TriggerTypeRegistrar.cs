using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// Register trigger types.
/// </summary>
/// <typeparam name="TDbContext">Database context type of the trigger.</typeparam>
/// <typeparam name="T">Type of trigger.</typeparam>
public class TriggerTypeRegistrar<TDbContext, T>
    where TDbContext : DbContext
    where T : class
{
    private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

    /// <summary>
    /// Assigns passed in triggerRegistrar.
    /// </summary>
    /// <param name="triggerRegistrar">Instance of <see cref="TriggerRegistrar{TDbContext}"/>.</param>
    internal TriggerTypeRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar)
    {
        _triggerRegistrar = triggerRegistrar;
    }

    /// <summary>
    /// InsertingAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> InsertingAsync
    {
        add => _triggerRegistrar.Register(TriggerType.Inserting, value);
        remove => _triggerRegistrar.Revoke(TriggerType.Inserting, value);
    }

    /// <summary>
    /// InsertedAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> InsertedAsync
    {
        add => _triggerRegistrar.Register(TriggerType.Inserted, value);
        remove => _triggerRegistrar.Revoke(TriggerType.Inserted, value);
    }

    /// <summary>
    /// UpdatingAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> UpdatingAsync
    {
        add => _triggerRegistrar.Register(TriggerType.Updating, value);
        remove => _triggerRegistrar.Revoke(TriggerType.Updating, value);
    }

    /// <summary>
    /// UpdatedAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> UpdatedAsync
    {
        add => _triggerRegistrar.Register(TriggerType.Updated, value);
        remove => _triggerRegistrar.Revoke(TriggerType.Updated, value);
    }

    /// <summary>
    /// DeletingAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> DeletingAsync
    {
        add => _triggerRegistrar.Register(TriggerType.Deleting, value);
        remove => _triggerRegistrar.Revoke(TriggerType.Deleting, value);
    }

    /// <summary>
    /// DeletedAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> DeletedAsync
    {
        add => _triggerRegistrar.Register(TriggerType.Deleted, value);
        remove => _triggerRegistrar.Revoke(TriggerType.Deleted, value);
    }
}
