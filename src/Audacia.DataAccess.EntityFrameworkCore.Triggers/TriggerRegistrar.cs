using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    internal static class TriggerRegistrar<TDbContext>
        where TDbContext : DbContext
    {
        private static readonly IDictionary<object, Action<object, TriggerContext<TDbContext>>> DelegateCache =
            new Dictionary<object, Action<object, TriggerContext<TDbContext>>>();

        private static readonly IDictionary<TriggerTypeHash, Action<object, TriggerContext<TDbContext>>> Triggers =
            new Dictionary<TriggerTypeHash, Action<object, TriggerContext<TDbContext>>>();

        internal static void Register<TEntity>(TriggerType type, Action<TEntity, TriggerContext<TDbContext>> action)
            where TEntity : class
        {
            var key = new TriggerTypeHash
            {
                EntityType = typeof(TEntity),
                TriggerType = type
            };

            void GenericisedAction(object obj, TriggerContext<TDbContext> context) => action(obj as TEntity, context);

            Triggers[key] += GenericisedAction;
            DelegateCache[action] = GenericisedAction;
        }

        internal static void Revoke<TEntity>(TriggerType type, Action<TEntity, TriggerContext<TDbContext>> action)
        {
            if (DelegateCache.TryGetValue(action, out var genericisedAction))
            {
                DelegateCache.Remove(genericisedAction);

                var key = new TriggerTypeHash
                {
                    EntityType = typeof(TEntity),
                    TriggerType = type
                };

                if (Triggers.ContainsKey(key))
                {
                    // ReSharper disable once DelegateSubtraction
                    Triggers[key] -= genericisedAction;
                }
            }
        }

        internal static IEnumerable<Action<object, TriggerContext<TDbContext>>> Resolve(Type entityType, TriggerType triggerType)
        {
            //NOTE: We want to match base types and interfaces too
            return from entry in Triggers
                where entry.Key.EntityType.IsAssignableFrom(entityType)
                      && entry.Key.TriggerType == triggerType
                orderby GetSortOrder(entry.Key, entityType)
                select entry.Value;
        }

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