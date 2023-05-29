using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to manage <see cref="IResolver"/>.
    /// </summary>
    public interface IScope :
        IDisposable
    {
        /// <summary>
        /// <see cref="IResolver"/> managed in this <see cref="IScope"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScope"/> is disposed.
        /// </exception>
        IResolver Resolver { get; }

        /// <summary>
        /// Creates a <see cref="IContext"/> to create a new child <see cref="IScope"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IContext"/> created.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IScope"/> is disposed.
        /// </exception>
        IContext CreateContext();
    }
}
