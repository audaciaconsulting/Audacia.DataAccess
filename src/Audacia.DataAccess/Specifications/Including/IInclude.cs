﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including;

/// <summary>
/// <see cref="IInclude{T}"/> intreface.
/// </summary>
/// <typeparam name="T">Type of <see cref="IInclude{T}"/>. </typeparam>
public interface IInclude<T>
{
    /// <summary>
    /// Specifies an include.
    /// </summary>
    /// <typeparam name="TKey">Output type of <see cref="Expression{T}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>. </param>
    /// <returns>Instance of <see cref="IThenInclude{TKey}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This is may code already using this.")]
    IThenInclude<TKey> With<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Specifies a collection to include.    
    /// </summary>
    /// <typeparam name="TKey">Output type of <see cref="Expression{T}"/>.</typeparam>
    /// <param name="keySelector">Collection of instances of <see cref="Expression{T}"/>. </param>
    /// <returns>Instance of <see cref="IThenInclude{TKey}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This is may code already using this.")]
    IThenInclude<TKey> With<TKey>(Expression<Func<T, ICollection<TKey>>> keySelector);
}