using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to register dependencies to <see cref="IScope"/>.
    /// </summary>
    public interface IContainer :
        ICompilation
    {
        /// <summary>
        /// Adds <see cref="IComposition"/> to resolve dependencies.
        /// </summary>
        /// <param name="composition">
        /// <see cref="IComposition"/> to resolve.
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
