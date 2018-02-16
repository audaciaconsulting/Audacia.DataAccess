using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Ordering
{
    /// <summary>
    /// The default implementation of <see cref="IOrderableQuerySpecification{T}"/> that contains the base <see cref="IQuerySpecification{T}"/> properties
    /// together with an <see cref="IOrderSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderableQuerySpecification<T> : IOrderableQuerySpecification<T>
    {
        public IFilterSpecification<T> Filter { get; set; }

        public IIncludeSpecification<T> Include { get; set; }

        public IOrderSpecification<T> Order { get; set; }

        public OrderableQuerySpecification(IOrderSpecification<T> orderSpecification)
        {
            Order = orderSpecification;
        }

        public OrderableQuerySpecification(IQuerySpecification<T> buildFrom, IOrderSpecification<T> orderSpecification)
        {
            Filter = buildFrom.Filter;
            Include = buildFrom.Include;
            Order = orderSpecification;
        }
    }

    /// <summary>
    /// The default implementation of <see cref="IOrderableQuerySpecification{T,TResult}"/> that contains 
    /// the base <see cref="IProjectableQuerySpecification{T,TResult}"/> properties together with an <see cref="IOrderSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class OrderableQuerySpecification<T, TResult> : IOrderableQuerySpecification<T, TResult> where T : class
    {
        public IFilterSpecification<T> Filter { get; set; }
        public IIncludeSpecification<T> Include { get; set; }
        public IProjectionSpecification<T, TResult> Projection { get; set; }
        public IOrderSpecification<TResult> Order { get; set; }

        public OrderableQuerySpecification(IProjectableQuerySpecification<T, TResult> buildFrom,
            IOrderSpecification<TResult> orderSpecification)
        {
            Filter = buildFrom.Filter;
            Include = buildFrom.Include;
            Projection = buildFrom.Projection;
            Order = orderSpecification;
        }
    }
}