using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to register dependencies in <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IDescription
    {
        /// <summary>
        /// <see cref="Type"/> to instantiate.
        /// </summary>
        Type ImplementedType { get; }

        /// <summary>
        /// How to manage lifetime for <see cref="ImplementedType"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// How to manage ownership for <see cref="ImplementedType"/>.
        /// </summary>
        Ownership Ownership { get; }

        /// <summary>
        /// Creates instance with <paramref name="resolver"/>.
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
