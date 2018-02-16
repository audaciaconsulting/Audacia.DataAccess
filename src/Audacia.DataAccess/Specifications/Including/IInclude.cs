using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including
{
    public interface IInclude<T>
    {
        /// <summary>
        /// Specifies an include
        /// </summary>
        IThenInclude<TKey> With<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}