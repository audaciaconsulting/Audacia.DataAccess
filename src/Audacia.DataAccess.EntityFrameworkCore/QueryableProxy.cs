using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore
{
    /// <summary>
    /// Wrapper class to allow <see cref="IQueryable{TEntity}"/> instances to be injected.
    /// The <see cref="IQueryable{TEntity}"/> implementation will be the corresponding <see cref="DbSet{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class QueryableProxy<TEntity, TContext> : IQueryable<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly IQueryable<TEntity> _dbSet;

        /// <summary>
        /// Initializes an instance of <see cref="QueryableProxy{TEntity, TContext}"/>.
        /// </summary>
        /// <param name="databaseContext">The database context containing the set of <typeparamref name="TEntity"/>.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="databaseContext"/> is <see langword="null"/>.</exception>
        public QueryableProxy(TContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));

            _dbSet = databaseContext.Set<TEntity>();
        }

        /// <inheritdoc />
        public Type ElementType => _dbSet.ElementType;

        /// <inheritdoc />
        public Expression Expression => _dbSet.Expression;

        /// <inheritdoc />
        public IQueryProvider Provider => _dbSet.Provider;

        /// <inheritdoc />
        public IEnumerator<TEntity> GetEnumerator()
        {
            return _dbSet.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
