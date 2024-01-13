using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for method injection.
    /// </summary>
    public sealed class MethodInjectionRequest
    {
        /// <summary>
        /// <see cref="Type"/> for implemented type.
        /// </summary>
        public Type ImplementedType { get; }

        /// <summary>
        /// <see cref="MethodInfo"/> for <see cref="ImplementedType"/>.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// <see cref="ParameterInfo"/>s for <see cref="ImplementedType"/>.
        /// </summary>
        public ParameterInfo[] ParameterList { get; }

        /// <summary>
        /// Constructor of <see cref="MethodInjectionRequest"/>.
        /// </summary>
        /// <param name="implementedType">
        /// <see cref="Type"/> for <see cref="ImplementedType"/>.
        /// </param>
        /// <param name="method">
        /// <see cref="MethodInfo"/> for <see cref="ImplementedType"/>.
        /// </param>
        public MethodInjectionRequest(Type implementedType, MethodInfo method)
        {
            ImplementedType = implementedType;
            Method = method;
            ParameterList = Method.GetParameters();
        }
    }
}
