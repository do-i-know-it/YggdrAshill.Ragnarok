using System;

namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines how to register dependencies to <see cref="IScope"/>.
    /// </summary>
    public interface IContainer :
        ICompilation
    {
        /// <summary>
        /// Adds a dependency.
        /// </summary>
        /// <param name="composition">
        /// <see cref="IComposition"/> to define a dependency.
        /// </param>
        void Register(IComposition composition);

        /// <summary>
        /// Adds a callback to execute after building.
        /// </summary>
        /// <param name="callback">
        /// <see cref="Action{T}"/> to execute with <see cref="IResolver"/>.
        /// </param>
        void Register(Action<IResolver> callback);
    }
}
