using System;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for property injection.
    /// </summary>
    public sealed class PropertyInjectionRequest
    {
        /// <summary>
        /// <see cref="Type"/> for implemented type.
        /// </summary>
        public Type ImplementedType { get; }

        /// <summary>
        /// <see cref="PropertyInfo"/>s for <see cref="ImplementedType"/>.
        /// </summary>
        public PropertyInfo[] PropertyList { get; }

        /// <summary>
        /// Constructor of <see cref="PropertyInjectionRequest"/>.
        /// </summary>
        /// <param name="implementedType">
        /// <see cref="Type"/> for <see cref="ImplementedType"/>.
        /// </param>
        /// <param name="propertyList">
        /// <see cref="PropertyInfo"/>s for <see cref="ImplementedType"/>.
        /// </param>
        public PropertyInjectionRequest(Type implementedType, PropertyInfo[] propertyList)
        {
            ImplementedType = implementedType;
            PropertyList = propertyList;
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
            if (PropertyList.Length == 0)
            {
                return WithoutDependency.Instance;
            }

            var argumentList = PropertyList.Select(info => new Argument(info.Name, info.PropertyType)).ToArray();
            return new WithDependency(argumentList);
        }
    }
}
