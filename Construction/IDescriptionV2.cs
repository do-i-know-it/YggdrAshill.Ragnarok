using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to declare a dependency.
    /// </summary>
    public interface IDescriptionV2
    {
        /// <summary>
        /// <see cref="Type"/> to instantiate.
        /// </summary>
        Type ImplementedType { get; }

        /// <summary>
        /// <see cref="Type"/>s to resolve.
        /// </summary>
        IReadOnlyList<Type> AssignedTypeList { get; }

        /// <summary>
        /// How to manage lifetime for <see cref="ImplementedType"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// How to manage ownership for <see cref="ImplementedType"/>.
        /// </summary>
        Ownership Ownership { get; }

        /// <summary>
        /// How to instantiate <see cref="ImplementedType"/>.
        /// </summary>
        IInstantiationV2 Instantiation { get; }
    }
}
