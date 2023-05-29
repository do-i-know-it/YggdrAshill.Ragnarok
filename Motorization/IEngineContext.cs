﻿using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to build <see cref="IEngine"/>.
    /// </summary>
    public interface IEngineContext :
        IEngineBuilder
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
        IEngine Build(IReadOnlyList<IDescription> descriptionList);
    }
}