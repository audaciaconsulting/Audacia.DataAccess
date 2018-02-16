using System;
using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Ordering
{
    public class DynamicOrderSpecification<T> : IOrderSpecification<T>
    {
        private readonly IBuildableOrderSpecification<T> _wrappedSpecification;

        public DynamicOrderSpecification(Action<IBuildableOrderSpecification<T>> orderAction)
        {
            _wrappedSpecification = OrderSpecification<T>.CreateInternal();

            orderAction(_wrappedSpecification);
        }

        public IEnumerable<OrderStep> OrderSteps => _wrappedSpecification.OrderSteps;
    }
}