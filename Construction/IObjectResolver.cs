using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to obtain instance resolved dependencies.
    /// </summary>
    public interface IObjectResolver
    {
        /// <summary>
        /// Gets instance of <paramref name="type"/>.
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
