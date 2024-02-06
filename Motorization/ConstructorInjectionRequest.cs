using System;
using System.Linq;
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
        /// <see cref="ParameterInfo"/>s for <see cref="ImplementedType"/>.
        /// </summary>
        public ParameterInfo[] ParameterList { get; }

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
            ParameterList = Constructor.GetParameters();
        }

        private IDependency? dependency;
        public IDependency Dependency
        {
            get
            {
                if (dependency == null)
                {
                    dependency = CreateDependency();
                }

                return dependency;
            }
        }
        private IDependency CreateDependency()
        {
            if (ParameterList.Length == 0)
            {
                return WithoutDependency.Instance;
            }

            var argumentList = ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();
            return new WithDependency(argumentList);
        }
    }
}
