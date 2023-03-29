using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines how to declare dependencies.
    /// </summary>
    public interface IDescription
    {
        /// <summary>
        /// Declared <see cref="Type"/>.
        /// </summary>
        Type ImplementedType { get; }

        /// <summary>
        /// <see cref="Type"/>s dependent on <see cref="ImplementedType"/>.
        /// </summary>
        IReadOnlyList<Type> AssignedTypeList { get; }

        /// <summary>
        /// <see cref="Lifetime"/> for <see cref="ImplementedType"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// <see cref="Ownership"/> for <see cref="ImplementedType"/>.
        /// </summary>
        Ownership Ownership { get; }

        /// <summary>
        /// How to instantiate <see cref="ImplementedType"/>.
        /// </summary>
        IInstantiation Instantiation { get; }
    }
}
