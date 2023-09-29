using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to manage <see cref="IObjectResolver"/>.
    /// </summary>
    public interface IObjectScope : IDisposable
    {
        /// <summary>
        /// <see cref="IObjectResolver"/> managed in this <see cref="IObjectScope"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IObjectScope"/> is disposed.
        /// </exception>
        IObjectResolver Resolver { get; }

        /// <summary>
        /// Creates <see cref="IObjectContext"/> to create new <see cref="IObjectScope"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IObjectContext"/> created.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IObjectScope"/> is disposed.
        /// </exception>
        IObjectContext CreateContext();
    }
}
