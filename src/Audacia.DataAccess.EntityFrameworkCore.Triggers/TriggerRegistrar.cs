using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerRegistrar<TDbContext>
        where TDbContext : DbContext
    {
        private class TriggerTypeHash : IEquatable<TriggerTypeHash>
        {
            public TriggerType TriggerType { get; set; }
            public Type EntityType { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as TriggerTypeHash);
            }

            public bool Equals(TriggerTypeHash other)
            {
                return other != null &&
                       TriggerType == other.TriggerType &&
                       EqualityComparer<Type>.Default.Equals(EntityType, other.EntityType);
            }

            public override int GetHashCode()
            {
                var hashCode = -1203416947;
                hashCode = hashCode * -1521134295 + TriggerType.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(EntityType);
                return hashCode;
            }

            public static bool operator ==(TriggerTypeHash hash1, TriggerTypeHash hash2)
            {
                return EqualityComparer<TriggerTypeHash>.Default.Equals(hash1, hash2);
            }

            public static bool operator !=(TriggerTypeHash hash1, TriggerTypeHash hash2)
            {
                return !(hash1 == hash2);
            }
        }

        private readonly IDictionary<object, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> _delegateCache =
            new Dictionary<object, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>>();

        private readonly IDictionary<TriggerTypeHash, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> _triggers =
            new Dictionary<TriggerTypeHash, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>>();

        private Func<TDbContext, CancellationToken, Task> _beforeAsync;
        private Func<TDbContext, CancellationToken, Task> _afterAsync;

        public event Func<TDbContext, CancellationToken, Task> BeforeAsync
        {
            add => _beforeAsync += value;
            // ReSharper disable once DelegateSubtraction
            remove => _beforeAsync -= value;
        }

        public event Func<TDbContext, CancellationToken, Task> AfterAsync
        {
            add => _afterAsync += value;
            // ReSharper disable once DelegateSubtraction
            remove => _afterAsync -= value;
        }
        
        public TriggerTypeRegistrar<TDbContext, T> Type<T>() where T : class
        {
            return new TriggerTypeRegistrar<TDbContext, T>(this);
        }

        public void Register<T>(TriggerType type, Func<T, TriggerContext<TDbContext>, CancellationToken, Task> action)
            where T : class
        {
            var key = new TriggerTypeHash
            {
                EntityType = typeof(T),
                TriggerType = type
            };

            Task GenericisedAction(object obj, TriggerContext<TDbContext> context,
                CancellationToken cancellationToken) => action(obj as T, context, cancellationToken);

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

                if (_triggers.ContainsKey(key))
                {
                    // ReSharper disable once DelegateSubtraction
                    _triggers[key] -= genericisedAction;
                }
            }
        }
        
        internal IEnumerable<Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> Resolve(Type entityType, TriggerType triggerType)
        {
            //NOTE: We want to match base types and interfaces too
            return from entry in _triggers
                where entry.Key.EntityType.IsAssignableFrom(entityType)
                      && entry.Key.TriggerType == triggerType
                orderby GetSortOrder(entry.Key, entityType)
                select entry.Value;
        }

        internal Func<TDbContext, CancellationToken, Task> ResolveBefore() => _beforeAsync;

        internal Func<TDbContext, CancellationToken, Task> ResolveAfter() => _afterAsync;

        private static int GetSortOrder(TriggerTypeHash key, Type type)
        {
            //Type itself always wins
            if (key.EntityType == type)
            {
                return 0;
            }

            //Base Classes beat interfaces
            return !key.EntityType.IsInterface ? 1 : 2;
        }
    }
}