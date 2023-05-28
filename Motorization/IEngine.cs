using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to realize features of <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IEngine :
        IDisposable
    {
        // TODO: rename method.
        /// <summary>
        /// Checks if this <see cref="IEngine"/> has <see cref="IRegistration"/>.
        /// </summary>
        /// <param name="registration">
        /// <see cref="IRegistration"/> to check.
        /// </param>
        /// <returns>
        /// True if this has <paramref name="registration"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IEngine"/> is disposed.
        /// </exception>
        bool Have(IRegistration registration);

        /// <summary>
        /// Tries to get <see cref="IRegistration"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to find <see cref="IRegistration"/>.
        /// </param>
        /// <param name="registration">
        /// <see cref="IRegistration"/> found.
        /// </param>
        /// <returns>
        /// True if this has <see cref="IRegistration"/> for <see cref="Type"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IEngine"/> is disposed.
        /// </exception>
        bool Find(Type type, out IRegistration registration);

        /// <summary>
        /// Obtains <see cref="object"/> with <see cref="IRegistration"/>.
        /// </summary>
        /// <param name="registration">
        /// <see cref="IRegistration"/> to instantiate.
        /// </param>
        /// <param name="factory">
        /// <see cref="Func{T,TResult}"/> to instantiate with <see cref="IRegistration"/>.
        /// </param>
        /// <returns>
        /// <see cref="object"/> obtained.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IEngine"/> is disposed.
        /// </exception>
        object GetInstance(IRegistration registration, Func<IRegistration, object> factory);

        /// <summary>
        /// Collects <see cref="IDisposable"/> to manage lifetime.
        /// </summary>
        /// <param name="disposable">
        /// <see cref="IDisposable"/> to bind.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if this <see cref="IEngine"/> is disposed.
        /// </exception>
        void Bind(IDisposable disposable);
    }
}
