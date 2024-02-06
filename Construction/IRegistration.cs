using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to register dependencies.
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Counts <see cref="IStatement"/> satisfied with <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="ICondition"/> to count.
        /// </param>
        /// <returns>
        /// Count of <see cref="IStatement"/>s.
        /// </returns>
        int Count(ICondition condition);

        /// <summary>
        /// Adds <see cref="IStatement"/> to register dependencies.
        /// </summary>
        /// <param name="statement">
        /// <see cref="IStatement"/> to register.
        /// </param>
        void Register(IStatement statement);

        /// <summary>
        /// Adds <see cref="IInstruction"/> to initialize <see cref="IObjectResolver"/>.
        /// </summary>
        /// <param name="instruction">
        /// <see cref="IInstruction"/> to register.
        /// </param>
        void Register(IInstruction instruction);

        /// <summary>
        /// Adds <see cref="IDisposable"/> to bind.
        /// </summary>
        /// <param name="disposable">
        /// <see cref="IDisposable"/> to register.
        /// </param>
        void Register(IDisposable disposable);
    }
}
