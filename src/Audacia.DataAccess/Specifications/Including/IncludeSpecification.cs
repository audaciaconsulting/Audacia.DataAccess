using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including
{
    public class IncludeSpecification<T> : IBuildableIncludeSpecification<T>
    {
        private readonly ICollection<IncludeStepPath> _includeStepPaths = new List<IncludeStepPath>();

        protected IncludeSpecification()
        {
        }

        internal static IncludeSpecification<T> CreateInternal()
        {
            return new IncludeSpecification<T>();
        }
        
        public IEnumerable<IncludeStepPath> IncludeStepPaths => _includeStepPaths;

        public IThenInclude<TKey> With<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            var path = new IncludeStepPath
            {
                new IncludeStep(keySelector)
            };

            _includeStepPaths.Add(path);
            
            return new ThenInclude<TKey>(path);
        }
        public IThenInclude<TKey> With<TKey>(Expression<Func<T, ICollection<TKey>>> keySelector)
        {
            var path = new IncludeStepPath
            {
                new IncludeStep(keySelector)
            };

            _includeStepPaths.Add(path);
            
            return new ThenInclude<TKey>(path);
        }

        private class ThenInclude<TThen> : IThenInclude<TThen>
        {
            private readonly IncludeStepPath _includeStepPath;

            internal ThenInclude(IncludeStepPath includeStepPath)
            {
                _includeStepPath = includeStepPath;
            }

            public IThenInclude<TKey> Then<TKey>(Expression<Func<TThen, TKey>> keySelector)
            {
                _includeStepPath.Add(new IncludeStep(keySelector));

                return new ThenInclude<TKey>(_includeStepPath);
            }
            public IThenInclude<TKey> Then<TKey>(Expression<Func<TThen, ICollection<TKey>>> keySelector)
            {
                _includeStepPath.Add(new IncludeStep(keySelector));

                return new ThenInclude<TKey>(_includeStepPath);
            }
        }
    }
}