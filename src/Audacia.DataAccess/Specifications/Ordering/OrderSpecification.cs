using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Ordering
{
    /// <summary>
    /// Class that allows <see cref="IOrderSpecification{T}"/> objects to be built and then executed using the <see cref="OrderSteps"/> property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderSpecification<T> : IBuildableOrderSpecification<T>, IBuildableOrderedOrderSpecification<T>
    {
        private List<OrderStep> _orderSteps = new List<OrderStep>();

        public IEnumerable<OrderStep> OrderSteps => _orderSteps;

        protected OrderSpecification()
        {
        }
        
        public static OrderSpecification<T> CreateInternal()
        {
            return new OrderSpecification<T>();
        }

        /// <summary>
        /// Factory method to create an <see cref="OrderSpecification{T}"/> from existing <see cref="IOrderSpecification{T}"/>s.
        /// Provides safety by returning the created specification as an <see cref="IBuildableOrderedOrderSpecification{T}"/> thus ensuring
        /// that only the appropriate methods can be called on the returned object.
        /// </summary>
        /// <param name="existingSpecifications"></param>
        /// <returns></returns>
        public static IBuildableOrderedOrderSpecification<T> From(params IOrderSpecification<T>[] existingSpecifications)
        {
            var newSpec = new OrderSpecification<T>();
            foreach (var orderSpecification in existingSpecifications)
            {
                newSpec._orderSteps.AddRange(orderSpecification.OrderSteps);
            }

            return newSpec;
        }

        public IBuildableOrderedOrderSpecification<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _orderSteps = new List<OrderStep> { new OrderStep(true, typeof(TKey), keySelector) };

            return this;
        }

        public IBuildableOrderedOrderSpecification<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _orderSteps = new List<OrderStep> { new OrderStep(false, typeof(TKey), keySelector) };

            return this;
        }

        IBuildableOrderedOrderSpecification<T> IBuildableOrderedOrderSpecification<T>.ThenAsc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _orderSteps.Add(new OrderStep(true, typeof(TKey), keySelector));

            return this;
        }

        IBuildableOrderedOrderSpecification<T> IBuildableOrderedOrderSpecification<T>.ThenDesc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _orderSteps.Add(new OrderStep(false, typeof(TKey), keySelector));

            return this;
        }
    }
}