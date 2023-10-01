using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines <see cref="IObjectResolver"/> chained hierarchically.
    /// </summary>
    public interface IScopedResolver : IObjectResolver, IDisposable
    {
        // TODO: add document comments.
        bool CanEscalate(out IScopedResolver resolver);

        // TODO: add document comments.
        bool CanResolve(Type type, out IDescription description);

        /// <summary>
        /// Resolves instance with <paramref name="description"/>.
        /// </summary>
        /// <param name="description">
        /// <see cref="IDescription"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="object"/> resolved.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        object Resolve(IDescription description);

        // TODO: add document comments.
        void Bind(IDisposable disposable);

        /// <summary>
        /// Creates a <see cref="IScopedResolverBuilder"/> to create new <see cref="IScopedResolver"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolverBuilder"/> created.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        IScopedResolverBuilder CreateBuilder();
    }
}
