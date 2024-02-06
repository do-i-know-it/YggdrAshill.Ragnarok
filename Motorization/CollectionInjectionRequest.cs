using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for collection injection.
    /// </summary>
    public sealed class CollectionInjectionRequest
    {
        /// <summary>
        /// <see cref="Type"/> for element of collection.
        /// </summary>
        public Type ElementType { get; }

        /// <summary>
        /// Constructor of <see cref="CollectionInjectionRequest"/>.
        /// </summary>
        /// <param name="elementType">
        /// <see cref="Type"/> for <see cref="ElementType"/>.
        /// </param>
        public CollectionInjectionRequest(Type elementType)
        {
            ElementType = elementType;
        }

        public IDependency Dependency => WithoutDependency.Instance;
    }
}
