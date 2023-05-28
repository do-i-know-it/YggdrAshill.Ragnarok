using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to build <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IScopedResolverContext :
        ICompilation
    {
        /// <summary>
        /// Creates <see cref="IScopedResolver"/> from <see cref="IDescription"/>s.
        /// </summary>
        /// <param name="descriptionList">
        /// <see cref="IDescription"/>s to build.
        /// </param>
        /// <returns>
        /// <see cref="IScopedResolver"/> created.
        /// </returns>
        IScopedResolver Build(IEnumerable<IDescription> descriptionList);
    }
}
