using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines dependency to resolve.
    /// </summary>
    public interface IDependency
    {
        /// <summary>
        /// <see cref="Type"/>s depending on.
        /// </summary>
        IReadOnlyList<Type> DependentTypeList { get; }

        /// <summary>
        /// Creates <see cref="IRealization"/> with <paramref name="parameterList"/>.
        /// </summary>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to create.
        /// </param>
        /// <returns>
        /// <see cref="IRealization"/> created.
        /// </returns>
        IRealization CreateRealization(IReadOnlyList<IParameter> parameterList);
    }
}
