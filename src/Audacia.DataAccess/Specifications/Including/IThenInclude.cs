using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including;

public interface IThenInclude<T>
{
    /// <summary>
    /// Specifies an include
    /// </summary>
    IThenInclude<TKey> Then<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Specifies a collection to include
    /// </summary>
    IThenInclude<TKey> Then<TKey>(Expression<Func<T, ICollection<TKey>>> keySelector);
}