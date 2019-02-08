using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core.Extensions;
using Audacia.Core;
using Audacia.DataAccess.Specifications;
using Audacia.DataAccess.Specifications.DataStoreImplementations;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.SqlServer
{
    public class ReadDataRepository<TContext> : IReadableDataRepository
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly StoredProcedureBuilder _storedProcedureBuilder;
        private bool _trackChanges;

        public ReadDataRepository(TContext context, StoredProcedureBuilder storedProcedureBuilder)
        {
            _context = context;
            _storedProcedureBuilder = storedProcedureBuilder;
        }

        public DisposableSwitch BeginTrackChanges()
        {
            return new DisposableSwitch(() => _trackChanges = true, () => _trackChanges = false);
        }

        public async Task<bool> AllAsync<T>(IQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Filter == null)
            {
                // no filter has been provided
                return false;
            }

            var query = ApplyIncludes(specification).AsNoTracking();

            return await query.AllAsync(specification.Filter.Expression, cancellationToken);
        }

        public async Task<bool> AnyAsync<T>(IQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var query = ApplyIncludesAndFilter(specification).AsNoTracking();

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync<T>(
            IQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()
        ) where T : class
        {
            var query = ApplyIncludesAndFilter(specification);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<T> GetAsync<T>(IOrderableQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var query = ApplyIncludesAndFilter(specification);

            if (specification.Order != null)
            {
                query = PerformOrdering(specification.Order, query);
            }

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(IOrderableQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var query = ApplyIncludesAndFilter(specification);

            if (specification.Order != null)
            {
                query = PerformOrdering(specification.Order, query);
            }

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<TResult> GetAsync<T, TResult>(IProjectableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            var projectedQuery = query.Select(specification.Projection.Expression);

            return await projectedQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
            IProjectableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            var projectedQuery = query.Select(specification.Projection.Expression);

            return await projectedQuery.ToArrayAsync(cancellationToken);
        }

        public async Task<TResult> GetAsync<T, TResult>(IOrderableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            var projectedQuery = query.Select(specification.Projection.Expression);

            if (specification.Order != null)
            {
                projectedQuery = PerformOrdering(specification.Order, projectedQuery);
            }

            return await projectedQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
            IOrderableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            var projectedQuery = query.Select(specification.Projection.Expression);

            if (specification.Order != null)
            {
                projectedQuery = PerformOrdering(specification.Order, projectedQuery);
            }

            return await projectedQuery.ToArrayAsync(cancellationToken);
        }

        public Task<IPage<T>> GetPageAsync<T>(IPageableQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Order == null)
            {
                throw new ArgumentNullException(nameof(specification.Order),
                    "Cannot page query with no order specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = PerformOrdering(specification.Order, query);

            return Task.FromResult<IPage<T>>(new Page<T>(query, specification.PagingRequest));
        }

        public Task<IPage<TResult>> GetPageAsync<T, TResult>(IPageableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            if (specification.Order == null)
            {
                throw new ArgumentNullException(nameof(specification.Order),
                    "Cannot page query with no order specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            var projectedQuery = query.Select(specification.Projection.Expression);

            if (specification.Order != null)
            {
                projectedQuery = PerformOrdering(specification.Order, projectedQuery);
            }

            return Task.FromResult<IPage<TResult>>(new Page<TResult>(projectedQuery, specification.PagingRequest));
        }



        public Task<IPage<T>> GetPageAsync<T>(ISortablePageableQuerySpecification<T> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            return Task.FromResult<IPage<T>>(new Page<T>(query, specification.SortablePagingRequest));
        }

        public Task<IPage<TResult>> GetPageAsync<T, TResult>(
            ISortablePageableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            var projectedQuery = query.Select(specification.Projection.Expression);

            return Task.FromResult<IPage<TResult>>(new Page<TResult>(projectedQuery,
                specification.SortablePagingRequest));
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(IDataStoreImplementedQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var commandText = _storedProcedureBuilder.GetQueryText(specification);

            var query = _context.Set<T>()
                .FromSql(commandText);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
            IDataStoreImplementedQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var commandText = _storedProcedureBuilder.GetQueryText(specification);

            var query = _context.Set<T>()
                .FromSql(commandText);

            if (!_trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.Select(specification.Projection.Expression).ToArrayAsync(cancellationToken);
        }

        protected IQueryable<T> ApplyIncludes<T>(IQuerySpecification<T> specification) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            if (specification.Include != null)
            {
                query = ApplyIncludes(specification.Include, query);
            }

            return query;
        }

        protected IQueryable<T> ApplyIncludesAndFilter<T>(IQuerySpecification<T> specification) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            if (specification.Include != null)
            {
                query = ApplyIncludes(specification.Include, query);
            }

            if (specification.Filter != null)
            {
                query = query.Where(specification.Filter.Expression);
            }

            return query;
        }

        private static IQueryable<T> ApplyIncludes<T>(IIncludeSpecification<T> includeSpecification,
            IQueryable<T> query)
            where T : class
        {
            foreach (var path in includeSpecification.IncludeStepPaths)
            {
                var parts = new List<string>();

                foreach (var step in path)
                {
                    parts.Add(ExpressionExtensions.GetPropertyInfo(step.Expression).Name);

                    var strPath = string.Join(".", parts);

                    query = query.Include(strPath);
                }
            }


            return query;
        }

        protected IOrderedQueryable<T> PerformOrdering<T>(IOrderSpecification<T> orderSpecification,
            IQueryable<T> query)
        {
            var steps = new Queue<OrderStep>(orderSpecification.OrderSteps);

            var orderedQuery = PerformOrderStep(query, steps.Dequeue());

            while (steps.Any())
            {
                orderedQuery = PerformOrderStep(orderedQuery, steps.Dequeue());
            }

            return orderedQuery;
        }

        private IOrderedQueryable<T> PerformOrderStep<T>(IOrderedQueryable<T> query, OrderStep orderStep)
        {
            var orderMethod = orderStep.Ascending ? nameof(Queryable.ThenBy) : nameof(Queryable.ThenByDescending);

            return PerformOrderStep(query, orderStep, orderMethod);
        }

        private IOrderedQueryable<T> PerformOrderStep<T>(IQueryable<T> query, OrderStep orderStep)
        {
            var orderMethod = orderStep.Ascending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);

            return PerformOrderStep(query, orderStep, orderMethod);
        }

        private static IOrderedQueryable<T> PerformOrderStep<T>(IQueryable<T> query, OrderStep orderStep,
            string orderMethod)
        {
            var method =
                typeof(Queryable).GetMethods().First(m => m.Name == orderMethod && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(T), orderStep.Type);

            return genericMethod.Invoke(null, new object[] {query, orderStep.Expression}) as IOrderedQueryable<T>;
        }
    }
}