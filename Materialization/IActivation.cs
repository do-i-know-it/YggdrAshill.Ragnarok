using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to create instance.
    /// </summary>
    public interface IActivation
    {
        /// <summary>
        /// <see cref="Argument"/>s to instantiate.
        /// </summary>
        IReadOnlyList<Argument> ArgumentList { get; }

        /// <summary>
        /// <see cref="Type"/>s to resolve.
        /// </summary>
        [Obsolete("Use ArgumentList instead.")]
        IReadOnlyList<Type> DependentTypeList { get; }

        /// <summary>
        /// Creates <see cref="object"/> with <see cref="IResolver"/> and <see cref="IParameter"/>s.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IResolver"/> to activate.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to activate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> created.
        /// </returns>
        [Obsolete("Use Activate(object[]) instead.")]
        object Activate(IResolver resolver, IReadOnlyList<IParameter> parameterList);

        /// <summary>
        /// Instantiates with parameter <see cref="object"/>s.
        /// </summary>
        /// <param name="parameterList">
        /// <see cref="object"/>s to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> instantiated.
        /// </returns>
        object Activate(object[] parameterList);
    }
}
