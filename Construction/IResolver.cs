using System;

namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines how to obtain objects resolved.
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Gets an instance of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="object"/> resolved.
        /// </returns>
        object Resolve(Type type);
    }
}
