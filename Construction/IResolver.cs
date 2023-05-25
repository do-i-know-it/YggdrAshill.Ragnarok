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
        /// <exception cref="RagnarokException">
        /// Thrown if something goes wrong in resolving dependencies.
        /// </exception>
        object Resolve(Type type);
    }
}
