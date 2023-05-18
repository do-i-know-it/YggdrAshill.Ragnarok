using YggdrAshill.Ragnarok.Construction;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to build <see cref="IRegistry"/>.
    /// </summary>
    public interface IRegistryBuilder :
        ICodeBuilder
    {
        /// <summary>
        /// Creates <see cref="IRegistry"/> from <see cref="IDescription"/>s.
        /// </summary>
        /// <param name="descriptionList">
        /// <see cref="IDescription"/>s to build.
        /// </param>
        /// <returns>
        /// <see cref="IRegistry"/> created.
        /// </returns>
        IRegistry Build(IEnumerable<IDescription> descriptionList);
    }
}
