using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines service lifecycle for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// Type of service lifecycle.
    /// </typeparam>
    public sealed class ServiceBundle<T>
        where T : notnull
    {
        /// <summary>
        /// <typeparamref name="T"/>s for service lifecycle.
        /// </summary>
        public IReadOnlyList<T> Package { get; }

        /// <summary>
        /// Constructor of <see cref="ServiceBundle{T}"/>.
        /// </summary>
        /// <param name="package">
        /// <see cref="IReadOnlyList{T}"/> for <see cref="Package"/>.
        /// </param>
        public ServiceBundle(IReadOnlyList<T> package)
        {
            Package = package;
        }
    }
}
