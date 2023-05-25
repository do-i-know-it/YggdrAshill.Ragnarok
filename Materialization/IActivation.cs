using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to create instance.
    /// </summary>
    public interface IActivation
    {
        /// <summary>
        /// <see cref="Argument"/>s to instantiate.
        /// </summary>
        IReadOnlyList<Argument> ArgumentList { get; }

        /// <summary>
        /// Instantiates with parameter <see cref="object"/>s.
        /// </summary>
        /// <param name="parameterList">
        /// <see cref="object"/>s to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> instantiated.
        /// </returns>
        object Activate(object[] parameterList);
    }
}
