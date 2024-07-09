using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Audacia.CodeAnalysis.Analyzers.Helpers.MethodLength;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// Extension class for database context.
/// </summary>
public static class DbContextExtensions
{
    private static readonly ConditionalWeakTable<DbContext, object> TriggerRegistrarConditionalWeakTable =
        new ConditionalWeakTable<DbContext, object>();

    /// <summary>
    /// Add <see cref="TriggerRegistrar{TDbContext}"/> to the TDbContext instance.
    /// </summary>
    /// <typeparam name="TDbContext">Type of database context.</typeparam>
    /// <param name="databaseContext">Instance of database context.</param>
    /// <param name="registrar">Instance of <see cref="TriggerRegistrar{TDbContext}"/>.</param>
    /// <returns>database context instance.</returns>
    public static TDbContext EnableTriggers<TDbContext>(
        this TDbContext databaseContext,
        TriggerRegistrar<TDbContext> registrar)
        where TDbContext : DbContext
    {
        TriggerRegistrarConditionalWeakTable.Add(databaseContext, registrar);

        return databaseContext;
    }

    /// <summary>
    /// Check if any triggers have been added.
    /// </summary>
    /// <typeparam name="TDbContext">Type of database context.</typeparam>
    /// <param name="databaseContext">Instance of database context.</param>
    /// <returns>database context instance.</returns>
    public static bool TriggersEnabled<TDbContext>(this TDbContext databaseContext)
        where TDbContext : DbContext
    {
        return GetTriggerRegistrar(databaseContext) != null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CS8603:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    private static TriggerRegistrar<TDbContext> GetTriggerRegistrar<TDbContext>(TDbContext databaseContext)
        where TDbContext : DbContext
    {
        if (!TriggerRegistrarConditionalWeakTable.TryGetValue(databaseContext, out var registrar))
        {
            throw new KeyNotFoundException(
                $"You must call {nameof(EnableTriggers)} on the context before you can use triggers.");
        }

        return registrar as TriggerRegistrar<TDbContext> ?? throw new ArgumentNullException(registrar.GetType().Name);  
    }

    /// <summary>
    /// Save trigger changes.
    /// </summary>
    /// <typeparam name="TDbContext">Type of database context.</typeparam>
    /// <param name="databaseContext">Instance of database context.</param>
    /// <param name="baseSaveChangesAsync">Function(bool, CancellationToken) Task(int).</param>
    /// <param name="cancellationToken">Cancellationtoken.</param>
    /// <returns>Task(int).</returns>
    public static Task<int> SaveChangesWithTriggersAsync<TDbContext>(
        this TDbContext databaseContext,
        Func<bool, CancellationToken, Task<int>> baseSaveChangesAsync,
        CancellationToken cancellationToken = default)
        where TDbContext : DbContext =>
        databaseContext.SaveChangesWithTriggersAsync(baseSaveChangesAsync, true, cancellationToken);

    /// <summary>
    /// Save trigger changes.
    /// </summary>
    /// <typeparam name="TDbContext">Type of database context.</typeparam>
    /// <param name="databaseContext">Instance of database context.</param>
    /// <param name="baseSaveChangesAsync">Function(bool, CancellationToken) Task(int).</param>
    /// <param name="shouldAcceptAllChangesOnSuccess">value of acceptAllChangesOnSuccess.</param>
    /// <param name="cancellationToken">Cancellationtoken.</param>
    /// <returns>Task(int).</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    [MaxMethodLength(15)]
    public static async Task<int> SaveChangesWithTriggersAsync<TDbContext>(
        this TDbContext databaseContext,
        Func<bool, CancellationToken, Task<int>> baseSaveChangesAsync,
        bool shouldAcceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(databaseContext);

        ArgumentNullException.ThrowIfNull(baseSaveChangesAsync);

        var registrar = GetTriggerRegistrar(databaseContext);

        var invoker = new TriggerInvoker<TDbContext>(databaseContext, registrar);

        int result;

        cancellationToken.ThrowIfCancellationRequested();

        using (var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await invoker.BeforeAsync(cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            result = await baseSaveChangesAsync(shouldAcceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            await invoker.AfterAsync(cancellationToken).ConfigureAwait(false);

            //Last chance
            cancellationToken.ThrowIfCancellationRequested();

            transaction.Commit();
        }

        return result;
    }
}
