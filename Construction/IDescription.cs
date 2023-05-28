using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to declare a dependency.
    /// </summary>
    public interface IDescription
    {
        /// <summary>
        /// Declared <see cref="Type"/>.
        /// </summary>
        Type ImplementedType { get; }

        /// <summary>
        /// <see cref="Type"/>s to resolve an instance of <see cref="ImplementedType"/>.
        /// </summary>
        IReadOnlyList<Type> AssignedTypeList { get; }

        /// <summary>
        /// <see cref="Construction.Lifetime"/> for <see cref="ImplementedType"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// <see cref="Construction.Ownership"/> for <see cref="ImplementedType"/>.
        /// </summary>
        Ownership Ownership { get; }

        /// <summary>
        /// How to instantiate <see cref="ImplementedType"/>.
        /// </summary>
        IInstantiation Instantiation { get; }
    }
}
