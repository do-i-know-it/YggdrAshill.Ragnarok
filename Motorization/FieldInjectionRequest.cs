using System;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for field injection.
    /// </summary>
    public sealed class FieldInjectionRequest
    {
        /// <summary>
        /// <see cref="Type"/> for implemented type.
        /// </summary>
        public Type ImplementedType { get; }

        /// <summary>
        /// <see cref="FieldInfo"/>s for <see cref="ImplementedType"/>.
        /// </summary>
        public FieldInfo[] FieldList { get; }

        /// <summary>
        /// Constructor of <see cref="FieldInjectionRequest"/>.
        /// </summary>
        /// <param name="implementedType">
        /// <see cref="Type"/> for <see cref="ImplementedType"/>.
        /// </param>
        /// <param name="fieldList">
        /// <see cref="FieldInfo"/>s for <see cref="ImplementedType"/>.
        /// </param>
        public FieldInjectionRequest(Type implementedType, FieldInfo[] fieldList)
        {
            ImplementedType = implementedType;
            FieldList = fieldList;
        }

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

                if (FieldList.Length == 0)
                {
                    return dependency = WithoutDependency.Instance;
                }

                var argumentList = FieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray();
                return dependency = new WithDependency(argumentList);
            }
        }
    }
}
