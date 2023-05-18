using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok.Hierarchization
{
    /// <summary>
    /// Defines how to register dependencies in <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Declared <see cref="Type"/>.
        /// </summary>
        Type ImplementedType { get; }

        /// <summary>
        /// <see cref="Construction.Lifetime"/> for <see cref="ImplementedType"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// <see cref="Construction.Ownership"/> for <see cref="ImplementedType"/>.
        /// </summary>
        Ownership Ownership { get; }

        /// <summary>
        /// Instantiates <see cref="ImplementedType"/> with <see cref="IScopedResolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IScopedResolver"/> to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> instantiated.
        /// </returns>
        object Instantiate(IScopedResolver resolver);
    }
}
