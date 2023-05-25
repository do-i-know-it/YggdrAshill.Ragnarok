using YggdrAshill.Ragnarok.Hierarchization;
using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to find <see cref="IRegistration"/> for <see cref="Type"/>
    /// </summary>
    public interface IRegistry :
        IDisposable
    {
        /// <summary>
        /// Checks if <see cref="IRegistry"/> has <see cref="IRegistration"/>.
        /// </summary>
        /// <param name="registration">
        /// <see cref="IRegistration"/> to check.
        /// </param>
        /// <returns>
        /// True if this <see cref="IRegistry"/> has <see cref="IRegistration"/>.
        /// </returns>
        bool Have(IRegistration registration);

        /// <summary>
        /// Finds <see cref="IRegistration"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to find.
        /// </param>
        /// <param name="registration">
        /// <see cref="IRegistration"/> found.
        /// </param>
        /// <returns>
        /// True if <see cref="IRegistry"/> has <see cref="IRegistration"/> for <see cref="Type"/>.
        /// </returns>
        bool Find(Type type, out IRegistration? registration);
    }
}
