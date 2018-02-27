using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public ReadDataRepository(TContext context, StoredProcedureBuilder storedProcedureBuilder)
        {
            _context = context;
            _storedProcedureBuilder = storedProcedureBuilder;
        }

        public async Task<T> GetAsync<T>(IOrderableQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var query = ApplyIncludesAndFilter(specification);

            if (specification.Order != null)
            {
                query = PerformOrdering(specification.Order, query);
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
            if (specification.Order != null)
            {
                throw new ArgumentNullException(nameof(specification.Order),
                    "Cannot page query with no order specification");
            }

            var query = ApplyIncludesAndFilter(specification);
            
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
            
            if (specification.Order != null)
            {
                throw new ArgumentNullException(nameof(specification.Order),
                    "Cannot page query with no order specification");
            }

            var query = ApplyIncludesAndFilter(specification);

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

            return Task.FromResult<IPage<T>>(new Page<T>(query, specification.SortablePagingRequest));
        }

        public Task<IPage<TResult>> GetPageAsync<T, TResult>(ISortablePageableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            if (specification.Projection == null)
            {
                throw new ArgumentNullException(nameof(specification.Projection),
                    "Cannot project with no projection specification");
            }

            var query = ApplyIncludesAndFilter(specification);

            var projectedQuery = query.Select(specification.Projection.Expression);

            return Task.FromResult<IPage<TResult>>(new Page<TResult>(projectedQuery, specification.SortablePagingRequest));
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(IDataStoreImplementedQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var commandText = _storedProcedureBuilder.GetQueryText(specification);

            return await _context.Set<T>()
                .FromSql(commandText)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
            IDataStoreImplementedQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var commandText = _storedProcedureBuilder.GetQueryText(specification);

            return await _context.Set<T>()
                .FromSql(commandText)
                .Select(specification.Projection.Expression)
                .ToArrayAsync(cancellationToken);
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

        private IQueryable<T> ApplyIncludes<T>(IIncludeSpecification<T> includeSpecification, IQueryable<T> query)
        {
            foreach (var path in includeSpecification.IncludeStepPaths)
            {
                var steps = new Queue<IncludeStep>(path);

                var step = steps.Dequeue();

                var unTypedQuery = PerformFirstIncludeStep(step, query);
                
                while (steps.Any())
                {
                    var previousStepPropertyType = step.Type;

                    step = steps.Dequeue();

                    unTypedQuery = PerformAdditionalIncludeSteps<T>(step, unTypedQuery, previousStepPropertyType);
                }

                query = unTypedQuery as IQueryable<T>;
            }

            return query;
        }

        private object PerformFirstIncludeStep<T>(IncludeStep step, IQueryable<T> query)
        {
            var method = typeof(EntityFrameworkQueryableExtensions).GetMethods().First(m =>
                m.Name == nameof(EntityFrameworkQueryableExtensions.Include) && m.GetGenericArguments().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(T), step.Type);

            return genericMethod.Invoke(null, new object[] { query, step.Expression });
        }

        private object PerformAdditionalIncludeSteps<T>(IncludeStep step, object unTypedQuery, Type previousStepPropertyType)
        {
            var method = typeof(EntityFrameworkQueryableExtensions).GetMethods().First(m =>
                m.Name == nameof(EntityFrameworkQueryableExtensions.ThenInclude) && m.GetGenericArguments().Length == 3);

            var genericMethod = method.MakeGenericMethod(typeof(T), previousStepPropertyType, step.Type);

            return genericMethod.Invoke(null, new[] { unTypedQuery, step.Expression });
        }

        protected IOrderedQueryable<T> PerformOrdering<T>(IOrderSpecification<T> orderSpecification, IQueryable<T> query)
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

        private static IOrderedQueryable<T> PerformOrderStep<T>(IQueryable<T> query, OrderStep orderStep, string orderMethod)
        {
            var method =
                typeof(Queryable).GetMethods().First(m => m.Name == orderMethod && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(T), orderStep.Type);

            return genericMethod.Invoke(null, new object[] {query, orderStep.Expression}) as IOrderedQueryable<T>;
        }
    }
}