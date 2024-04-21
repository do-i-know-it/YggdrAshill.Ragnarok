using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for constructor injection.
    /// </summary>
    public sealed class ConstructorInjectionRequest
    {
        /// <summary>
        /// <see cref="Type"/> for implemented type.
        /// </summary>
        public Type ImplementedType { get; }

        /// <summary>
        /// <see cref="ConstructorInfo"/> for <see cref="ImplementedType"/>.
        /// </summary>
        public ConstructorInfo Constructor { get; }

        /// <summary>
        /// Constructor of <see cref="ConstructorInjectionRequest"/>.
        /// </summary>
        /// <param name="implementedType">
        /// <see cref="Type"/> for <see cref="ImplementedType"/>.
        /// </param>
        /// <param name="constructor">
        /// <see cref="ConstructorInfo"/> for <see cref="ImplementedType"/>.
        /// </param>
        public ConstructorInjectionRequest(Type implementedType, ConstructorInfo constructor)
        {
            ImplementedType = implementedType;
            Constructor = constructor;
        }

        private ParameterInfo[]? parameterList;

        /// <summary>
        /// <see cref="ParameterInfo"/>s for <see cref="ImplementedType"/>.
        /// </summary>
        public ParameterInfo[] ParameterList => parameterList ??= Constructor.GetParameters();
    }
}
