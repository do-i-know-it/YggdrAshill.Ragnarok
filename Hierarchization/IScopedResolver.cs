using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines <see cref="IObjectResolver"/> chained hierarchically.
    /// </summary>
    public interface IScopedResolver : IObjectResolver, IDisposable
    {
        /// <summary>
        /// Escalates <see cref="IScopedResolver"/> if this can.
        /// </summary>
        /// <param name="resolver">
        /// Parent <see cref="IScopedResolver"/>.
        /// </param>
        /// <returns>
        /// True if this has parent <see cref="IScopedResolver"/>.
        /// </returns>
        bool CanEscalate(out IScopedResolver resolver);

        /// <summary>
        /// Resolve <see cref="IDescription"/> of <see cref="Type"/> if this can.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to resolve.
        /// </param>
        /// <param name="description">
        /// <see cref="IDescription"/> resolved.
        /// </param>
        /// <returns>
        /// True if this has <see cref="IDescription"/>.
        /// </returns>
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

        /// <summary>
        /// Adds <see cref="IDisposable"/> to dispose when this disposes.
        /// </summary>
        /// <param name="disposable">
        /// <see cref="IDisposable"/> to bind.
        /// </param>
        void Bind(IDisposable disposable);

        /// <summary>
        /// Creates <see cref="IScopedResolverContext"/> to create new <see cref="IScopedResolver"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScopedResolver"/> is disposed.
        /// </exception>
        IScopedResolverContext CreateContext();
    }
}
