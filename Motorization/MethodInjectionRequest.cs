using System;
using System.Linq;
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
        }

        private ParameterInfo[]? parameterList;

        /// <summary>
        /// <see cref="ParameterInfo"/>s for <see cref="ImplementedType"/>.
        /// </summary>
        public ParameterInfo[] ParameterList => parameterList ??= Method.GetParameters();

        private IDependency? dependency;

        /// <summary>
        /// <see cref="IDependency"/> for <see cref="ImplementedType"/>.
        /// </summary>
        public IDependency Dependency
        {
            get
            {
                if (dependency != null)
                {
                    return dependency;
                }

                if (ParameterList.Length == 0)
                {
                    return dependency = WithoutDependency.Instance;
                }

                var argumentList = ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();
                return dependency = new WithDependency(argumentList);
            }
        }
    }
}
