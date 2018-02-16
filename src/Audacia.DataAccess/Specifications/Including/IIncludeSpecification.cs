using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Including
{
    /// <summary>
    /// An interface for specifying includes for data contexts
    /// </summary>
    /// <typeparam name="T">The entity whose relations we are including</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IIncludeSpecification<T>
    {

        /// <summary>
        /// Gets the <see cref="IncludeStep"/> objects that contain the include rules.
        /// </summary>
        IEnumerable<IncludeStepPath> IncludeStepPaths { get; }
    }
}