using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines <see cref="IObjectResolver"/> chained hierarchically.
    /// </summary>
    public interface IScopedResolverV2 : IObjectResolver, IDisposable
    {
        // TODO: add document comments.
        bool CanEscalate(out IScopedResolverV2 resolver);

        // TODO: add document comments.
        bool CanResolve(Type type, out IDepiction depiction);

        /// <summary>
        /// Resolves instance with <paramref name="depiction"/>.
        /// </summary>
        /// <param name="depiction">
        /// <see cref="IDepiction"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="object"/> resolved.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolverV2"/> is disposed.
        /// </exception>
        object Resolve(IDepiction depiction);

        // TODO: add document comments.
        void Bind(IDisposable disposable);

        /// <summary>
        /// Creates a <see cref="IScopedResolverBuilder"/> to create new <see cref="IScopedResolverV2"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolverBuilder"/> created.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolverV2"/> is disposed.
        /// </exception>
        IScopedResolverBuilder CreateBuilder();
    }
}
