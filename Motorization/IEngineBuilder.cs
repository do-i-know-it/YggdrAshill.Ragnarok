using YggdrAshill.Ragnarok.Construction;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Motorization
{
    /// <summary>
    /// Defines how to build <see cref="IEngine"/>.
    /// </summary>
    public interface IEngineBuilder :
        ICompilation
    {
        /// <summary>
        /// Creates <see cref="IEngine"/> from <see cref="IDescription"/>s.
        /// </summary>
        /// <param name="descriptionList">
        /// <see cref="IDescription"/>s to build.
        /// </param>
        /// <returns>
        /// <see cref="IEngine"/> created.
        /// </returns>
        IEngine Build(IEnumerable<IDescription> descriptionList);
    }
}
