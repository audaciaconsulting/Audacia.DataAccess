using System;
using System.Linq.Expressions;
using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications
{
    public static class QuerySpecificationExtensions
    {
        public static IQuerySpecification<T> AddFilter<T>(this IQuerySpecification<T> querySpecification,
            IFilterSpecification<T> filterSpecification)
            where T : class
        {
            querySpecification.Filter = querySpecification.Filter == null
                ? filterSpecification
                : querySpecification.Filter.And(filterSpecification);

            return querySpecification;
        }

        public static IQuerySpecification<T> AddFilter<T>(this IQuerySpecification<T> querySpecification,
            Expression<Func<T, bool>> filterExpression)
            where T : class
        {
            var filterSpecification = new DynamicFilterSpecification<T>(filterExpression);

            return querySpecification.AddFilter(filterSpecification);
        }

        public static IQuerySpecification<T> WithInclude<T>(this IQuerySpecification<T> querySpecification,
            IIncludeSpecification<T> includeSpecification)
        {
            querySpecification.Include = includeSpecification;

            return querySpecification;
        }

        public static IQuerySpecification<T> WithInclude<T>(this IQuerySpecification<T> querySpecification,
            Action<IBuildableIncludeSpecification<T>> includeAction)
        {
            querySpecification.Include = new DynamicIncludeSpecification<T>(includeAction);

            return querySpecification;
        }

        public static IOrderableQuerySpecification<T> WithOrder<T>(
            this IOrderableQuerySpecification<T> querySpecification, IOrderSpecification<T> orderSpecification)
        {
            querySpecification.Order = orderSpecification;

            return querySpecification;
        }

        public static IOrderableQuerySpecification<T> WithOrder<T>(
            this IOrderableQuerySpecification<T> querySpecification,
            Action<IBuildableOrderSpecification<T>> orderAction)
        {
            querySpecification.Order = new DynamicOrderSpecification<T>(orderAction);

            return querySpecification;
        }

        public static IProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(
            this IQuerySpecification<T> querySpecification,
            IProjectionSpecification<T, TResult> projectionSpecification) where T : class
        {
            return new ProjectableQuerySpecification<T, TResult>(querySpecification, projectionSpecification);
        }

        public static IProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(
            this IQuerySpecification<T> querySpecification, Expression<Func<T, TResult>> projectionExpression)
            where T : class
        {
            return new ProjectableQuerySpecification<T, TResult>(querySpecification,
                new DynamicProjectionSpecification<T, TResult>(projectionExpression));
        }

        public static ISortablePageableQuerySpecification<T> WithPaging<T>(
            this IQuerySpecification<T> querySpecification,
            SortablePagingRequest sortablePagingRequest) where T : class
        {
            return new SortablePageableQuerySpecification<T>(querySpecification, sortablePagingRequest);
        }

        public static IPageableQuerySpecification<T> WithPaging<T>(
            this IOrderableQuerySpecification<T> querySpecification,
            PagingRequest pagingRequest) where T : class
        {
            return new PageableQuerySpecification<T>(querySpecification, pagingRequest);
        }

        public static IPageableQuerySpecification<T, TResult> WithPaging<T, TResult>(
            this IOrderableQuerySpecification<T, TResult> querySpecification,
            PagingRequest pagingRequest) where T : class
        {
            return new PageableQuerySpecification<T, TResult>(querySpecification, pagingRequest);
        }

        public static IOrderableQuerySpecification<T, TResult> WithOrder<T, TResult>(
            this IProjectableQuerySpecification<T, TResult> querySpecification,
            IOrderSpecification<TResult> orderSpecification) where T : class
        {
            return new OrderableQuerySpecification<T, TResult>(querySpecification, orderSpecification);
        }

        public static SortablePageableQuerySpecification<T, TResult> WithPaging<T, TResult>(
            this IProjectableQuerySpecification<T, TResult> querySpecification,
            SortablePagingRequest sortablePagingRequest) where T : class
        {
            return new SortablePageableQuerySpecification<T, TResult>(querySpecification, sortablePagingRequest);
        }
    }
}