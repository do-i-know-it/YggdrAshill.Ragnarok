using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Hierarchization
{
    /// <summary>
    /// Defines <see cref="IResolver"/> chained hierarchically.
    /// </summary>
    public interface IScopedResolver :
        IResolver,
        IDisposable
    {
        /// <summary>
        /// Resolves dependencies with this or child <see cref="IScopedResolver"/> to instantiate.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to resolve.
        /// </param>
        /// <param name="child">
        /// Child <see cref="IScopedResolver"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="object"/> resolved.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        object Resolve(Type type, IScopedResolver child);

        /// <summary>
        /// Resolves dependencies with <see cref="IRegistration"/> from outer.
        /// </summary>
        /// <param name="registration">
        /// <see cref="IRegistration"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="object"/> resolved.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        object Resolve(IRegistration registration);

        /// <summary>
        /// Resolves all <see cref="IRegistration"/>s for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="IRegistration"/>s resolved.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        IEnumerable<IRegistration> ResolveAll(Type type);

        /// <summary>
        /// Creates a <see cref="IScopedResolverContext"/> to create a new child <see cref="IScopedResolver"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolverContext"/> created.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        IScopedResolverContext CreateContext();
    }
}
