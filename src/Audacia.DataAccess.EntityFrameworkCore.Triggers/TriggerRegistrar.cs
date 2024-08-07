﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// TriggerRegistrar class.
/// </summary>
/// <typeparam name="TDbContext">Type of <see cref="TriggerRegistrar{TDbContext}"/>.</typeparam>
public class TriggerRegistrar<TDbContext>
    where TDbContext : DbContext
{
    private class TriggerTypeHash : IEquatable<TriggerTypeHash>
    {
        public TriggerType TriggerType { get; set; }

        public Type? EntityType { get; set; }

        public override bool Equals(object? obj)
        {
           return Equals(obj as TriggerTypeHash);
        }

        public bool Equals(TriggerTypeHash? other)
        {
            return other != null && TriggerType == other.TriggerType &&
                   EqualityComparer<Type>.Default.Equals(EntityType, other.EntityType);
        }

        public override int GetHashCode()
        {
            var hashCode = -1203416947;
            if (EntityType != null)
            {
                hashCode = hashCode * -1521134295 + TriggerType.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(EntityType);
            }

            return hashCode;
        }

        public static bool operator ==(TriggerTypeHash? hashOne, TriggerTypeHash? hashTwo)
        {
            return EqualityComparer<TriggerTypeHash>.Default.Equals(hashOne, hashTwo);
        }

        public static bool operator !=(TriggerTypeHash? hashOne, TriggerTypeHash? hashTwo)
        {
            return !(hashOne == hashTwo);
        }
    }

    private readonly IDictionary<object, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> _delegateCache =
        new ConcurrentDictionary<object, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>>();

    private readonly IDictionary<TriggerTypeHash, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> _triggers =
        new ConcurrentDictionary<TriggerTypeHash, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>>();

    private Func<TDbContext, CancellationToken, Task>? _beforeAsync;
    private Func<TDbContext, CancellationToken, Task>? _afterAsync;

    /// <summary>
    /// BeforeAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<TDbContext, CancellationToken, Task> BeforeAsync
    {
        add => _beforeAsync += value;
        // ReSharper disable once DelegateSubtraction
        remove => _beforeAsync -= value;
    }

    /// <summary>
    /// AfterAsync event handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Breaking changes to the code.")]
    public event Func<TDbContext, CancellationToken, Task> AfterAsync
    {
        add => _afterAsync += value;
        // ReSharper disable once DelegateSubtraction
        remove => _afterAsync -= value;
    }

    /// <summary>
    /// Creates a new <see cref="TriggerTypeRegistrar{TDbContext,T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="TriggerTypeRegistrar{TDbContext,T}"/>.</typeparam>
    /// <returns>Instance of <see cref="TriggerTypeRegistrar{TDbContext,T}"/>.</returns>
    public TriggerTypeRegistrar<TDbContext, T> Type<T>() where T : class
    {
        return new TriggerTypeRegistrar<TDbContext, T>(this);
    }

    /// <summary>
    /// Register trigger type.
    /// </summary>
    /// <typeparam name="T">Entity type of <see cref="TriggerTypeHash"/>.</typeparam>
    /// <param name="type">Enum value of <see cref="TriggerType"/>. </param>
    /// <param name="action">Action delegate.</param>
    public void Register<T>(TriggerType type, Func<T, TriggerContext<TDbContext>, CancellationToken, Task> action)
        where T : class
    {
        var key = new TriggerTypeHash
        {
            EntityType = typeof(T),
            TriggerType = type
        };

        Task GenericisedAction(object obj, TriggerContext<TDbContext> context,
            CancellationToken cancellationToken) => action((obj as T)!, context, cancellationToken);

        if (!_triggers.ContainsKey(key))
        {
            _triggers[key] = GenericisedAction;
        }
        else
        {
            _triggers[key] += GenericisedAction;
        }

        _delegateCache[action] = GenericisedAction;
    }

    /// <summary>
    /// Revoke registered trigger type.
    /// </summary>
    /// <typeparam name="T">Entity type of <see cref="TriggerTypeHash"/>.</typeparam>
    /// <param name="type">Enum value of <see cref="TriggerType"/>. </param>
    /// <param name="action">Action delegate.</param>
    public void Revoke<T>(TriggerType type, Func<T, TriggerContext<TDbContext>, CancellationToken, Task> action)
    {
        if (_delegateCache.TryGetValue(action, out var genericisedAction))
        {
            _delegateCache.Remove(genericisedAction);

            var key = new TriggerTypeHash
            {
                EntityType = typeof(T),
                TriggerType = type
            };

            _triggers.TryGetValue(key, out var trigger);
            if (trigger != null)
            {
                trigger -= genericisedAction;
            }
        }
    }

    /// <summary>
    /// Resolve delegates.
    /// </summary>
    /// <param name="entityType">Type of entity.</param>
    /// <param name="triggerType">Type of trigger.</param>
    /// <returns>Collection of delegates .</returns>
    internal IEnumerable<Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> Resolve(Type entityType, TriggerType triggerType)
    {
        //NOTE: We want to match base types and interfaces too
        return from entry in _triggers
               where entry.Key.EntityType!.IsAssignableFrom(entityType)
                     && entry.Key.TriggerType == triggerType
               orderby GetSortOrder(entry.Key, entityType)
               select entry.Value;
    }

    /// <summary>
    /// Gets beforeAsync delegate.
    /// </summary>
    /// <returns>BeforeAsync delegate.</returns>
    internal Func<TDbContext, CancellationToken, Task>? ResolveBefore() => _beforeAsync;

    /// <summary>
    /// Gets afterAsync delegate.
    /// </summary>
    /// <returns>AfterAsync delegate.</returns>
    internal Func<TDbContext, CancellationToken, Task>? ResolveAfter() => _afterAsync;

    private static int GetSortOrder(TriggerTypeHash key, Type type)
    {
        //Type itself always wins
        if (key.EntityType == type)
        {
            return 0;
        }

        //Base Classes beat interfaces
        return !key.EntityType!.IsInterface ? 1 : 2;
    }
}
