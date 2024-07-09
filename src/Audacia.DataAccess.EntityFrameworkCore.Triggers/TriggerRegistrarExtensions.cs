using System;
using System.Threading.Tasks;
using Audacia.Core;
using Audacia.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// Extesntions for TriggerRegistrar.
/// </summary>
public static class TriggerRegistrarExtensions
{
    /// <summary>
    /// AddSoftDeleteTrigger extenstion.
    /// </summary>
    /// <typeparam name="TUserIdentifier">Struct type.</typeparam>
    /// <typeparam name="TDbContext">Database context type.</typeparam>
    /// <param name="registrar">Instance of <see cref="TriggerRegistrar{TDbContext}"/>.</param>
    /// <param name="userIdentifierFactory">Func delegate <see cref="Func{TUserIdentifier}"/>.</param>
    public static void AddSoftDeleteTrigger<TUserIdentifier, TDbContext>(
        this TriggerRegistrar<TDbContext> registrar,
        Func<TUserIdentifier?> userIdentifierFactory)
        where TUserIdentifier : struct
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(registrar);   
        registrar.Type<ISoftDeletable<TUserIdentifier>>().DeletingAsync += (deletable, context, _) =>
        {
            context.EntityEntry.State = EntityState.Modified;
            deletable.Deleted = DateTimeOffsetProvider.Instance.Now;
            deletable.DeletedBy = userIdentifierFactory();

            return Task.CompletedTask;
        };
    }

    /// <summary>
    /// AddCreateTrigger extension.
    /// </summary>
    /// <typeparam name="TUserIdentifier">Struct type.</typeparam>
    /// <typeparam name="TDbContext">Database context type.</typeparam>
    /// <param name="registrar">Instance of <see cref="TriggerRegistrar{TDbContext}"/>.</param>
    /// <param name="userIdentifierFactory">Func delegate <see cref="Func{TUserIdentifier}"/>.</param>
    public static void AddCreateTrigger<TUserIdentifier, TDbContext>(
        this TriggerRegistrar<TDbContext> registrar,
        Func<TUserIdentifier?> userIdentifierFactory)
        where TUserIdentifier : struct
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(registrar);
        registrar.Type<ICreatable<TUserIdentifier>>().InsertingAsync += (creatable, _, _) =>
        {
            creatable.Created = DateTimeOffsetProvider.Instance.Now;
            creatable.CreatedBy = userIdentifierFactory();

            return Task.CompletedTask;
        };
    }

    /// <summary>
    /// AddModifyTrigger extension.
    /// </summary>
    /// <typeparam name="TUserIdentifier">Struct type.</typeparam>
    /// <typeparam name="TDbContext">Database context type.</typeparam>
    /// <param name="registrar">Instance of <see cref="TriggerRegistrar{TDbContext}"/>.</param>
    /// <param name="userIdentifierFactory">Func delegate <see cref="Func{TUserIdentifier}"/>.</param>
    public static void AddModifyTrigger<TUserIdentifier, TDbContext>(
        this TriggerRegistrar<TDbContext> registrar,
        Func<TUserIdentifier?> userIdentifierFactory)
        where TUserIdentifier : struct
        where TDbContext : DbContext        
    {
        ArgumentNullException.ThrowIfNull(registrar);
        registrar.Type<IModifiable<TUserIdentifier>>().UpdatingAsync += (modifiable, _, _) =>
        {
            modifiable.Modified = DateTimeOffsetProvider.Instance.Now;
            modifiable.ModifiedBy = userIdentifierFactory();

            return Task.CompletedTask;
        };
    }
}
