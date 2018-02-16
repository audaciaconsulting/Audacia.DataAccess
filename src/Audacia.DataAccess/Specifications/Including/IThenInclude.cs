using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including
{
    public interface IThenInclude<T>
    {
        /// <summary>
        /// Specifies an include
        /// </summary>
        IThenInclude<TKey> Then<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}