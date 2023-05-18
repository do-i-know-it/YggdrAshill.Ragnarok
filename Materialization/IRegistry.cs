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
        bool TryGet(Type type, out IRegistration? registration);
    }
}
