using System;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerRegistrar
    {
        private readonly IDictionary<object, Action<object, TriggerContext>> _delegateCache =
            new Dictionary<object, Action<object, TriggerContext>>();

        private readonly IDictionary<TriggerTypeHash, Action<object, TriggerContext>> _triggers =
            new Dictionary<TriggerTypeHash, Action<object, TriggerContext>>();

        public void Register<TEntity>(TriggerType type, Action<TEntity, TriggerContext> action)
            where TEntity : class
        {
            var key = new TriggerTypeHash
            {
                EntityType = typeof(TEntity),
                TriggerType = type
            };

            void GenericisedAction(object obj, TriggerContext context) => action(obj as TEntity, context);

            _triggers[key] += GenericisedAction;
            _delegateCache[action] = GenericisedAction;
        }

        public void Revoke<TEntity>(TriggerType type, Action<TEntity, TriggerContext> action)
        {
            if (_delegateCache.TryGetValue(action, out var genericisedAction))
            {
                _delegateCache.Remove(genericisedAction);

                var key = new TriggerTypeHash
                {
                    EntityType = typeof(TEntity),
                    TriggerType = type
                };

                if (_triggers.ContainsKey(key))
                {
                    // ReSharper disable once DelegateSubtraction
                    _triggers[key] -= genericisedAction;
                }
            }
        }

        public IEnumerable<Action<object, TriggerContext>> Resolve(Type entityType, TriggerType triggerType)
        {
            //NOTE: We want to match base types and interfaces too
            return from entry in _triggers
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