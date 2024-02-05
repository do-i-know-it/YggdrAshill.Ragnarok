using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for dependency injection.
    /// </summary>
    public sealed class DependencyInjectionRequest
    {
        /// <summary>
        /// <see cref="Type"/> to resolve.
        /// </summary>
        public Type ImplementedType { get; }

        /// <summary>
        /// <see cref="IParameter"/>s to resolve.
        /// </summary>
        public IReadOnlyList<IParameter> ParameterList { get; }

        /// <summary>
        /// Constructor of <see cref="DependencyInjectionRequest"/>.
        /// </summary>
        /// <param name="implementedType">
        /// <see cref="Type"/> for <see cref="ImplementedType"/>.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s for <see cref="ParameterList"/>.
        /// </param>
        public DependencyInjectionRequest(Type implementedType, params IParameter[] parameterList)
        {
            ImplementedType = implementedType;
            ParameterList = parameterList;
        }
    }
}
