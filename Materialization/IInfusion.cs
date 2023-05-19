using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to inject dependencies into instance.
    /// </summary>
    public interface IInfusion
    {
        /// <summary>
        /// <see cref="Argument"/>s to inject.
        /// </summary>
        IReadOnlyList<Argument> ArgumentList { get; }

        /// <summary>
        /// <see cref="Type"/>s to resolve.
        /// </summary>
        [Obsolete("Use ArgumentList instead.")]
        IReadOnlyList<Type> DependentTypeList { get; }

        /// <summary>
        /// Inject dependencies into <see cref="object"/> with <see cref="IResolver"/> and <see cref="IParameter"/>s.
        /// </summary>
        /// <param name="instance">
        /// <see cref="object"/> to infuse.
        /// </param>
        /// <param name="resolver">
        /// <see cref="IResolver"/> to infuse.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to infuse.
        /// </param>
        [Obsolete("Use Infuse(object, object[]) instead.")]
        void Infuse(object instance, IResolver resolver, IReadOnlyList<IParameter> parameterList);

        /// <summary>
        /// Injects <see cref="object"/>s into instance.
        /// </summary>
        /// <param name="instance">
        /// <see cref="object"/> to inject dependencies into.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="object"/>s to inject instance.
        /// </param>
        void Infuse(object instance, object[] parameterList);
    }
}
