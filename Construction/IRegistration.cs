using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to register dependencies.
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Counts <see cref="IStatement"/> satisfied with <see cref="IStatementSelection"/>.
        /// </summary>
        /// <param name="selection">
        /// <see cref="IStatementSelection"/> to count.
        /// </param>
        /// <returns>
        /// Count of <see cref="IStatement"/>s.
        /// </returns>
        int Count(IStatementSelection selection);

        /// <summary>
        /// Adds <see cref="IStatement"/> to register dependencies.
        /// </summary>
        /// <param name="statement">
        /// <see cref="IStatement"/> to register.
        /// </param>
        void Register(IStatement statement);

        /// <summary>
        /// Adds <see cref="IOperation"/> to initialize <see cref="IObjectResolver"/>.
        /// </summary>
        /// <param name="operation">
        /// <see cref="IOperation"/> to register.
        /// </param>
        void Register(IOperation operation);

        /// <summary>
        /// Adds <see cref="IDisposable"/> to bind.
        /// </summary>
        /// <param name="disposable">
        /// <see cref="IDisposable"/> to register.
        /// </param>
        void Register(IDisposable disposable);
    }
}
