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
        /// <see cref="Type"/>s to resolve.
        /// </summary>
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
        void Infuse(object instance, IResolver resolver, IReadOnlyList<IParameter> parameterList);
    }
}
