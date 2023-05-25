using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to inject dependencies into instance.
    /// </summary>
    public interface IInfusion
    {
        /// <summary>
        /// <see cref="Argument"/>s to inject.
        /// </summary>
        IReadOnlyList<Argument> ArgumentList { get; }

        /// <summary>
        /// Injects <see cref="object"/>s into instance.
        /// </summary>
        /// <param name="instance">
        /// <see cref="object"/> to inject dependencies into.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="object"/>s to inject instance.
        /// </param>
        void Infuse(object instance, object[] parameterList);
    }
}
