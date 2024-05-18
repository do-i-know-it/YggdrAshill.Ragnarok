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
        /// <see cref="IStatementCondition"/> to count.
        /// </param>
        /// <returns>
        /// Count of <see cref="IStatement"/>s.
        /// </returns>
        int Count(IStatementCondition condition);

        /// <summary>
        /// Adds <see cref="IStatement"/> to register dependencies.
        /// </summary>
        /// <param name="statement">
        /// <see cref="IStatement"/> to register.
        /// </param>
        void Register(IStatement statement);

        /// <summary>
        /// Adds <see cref="IExecution"/> to initialize <see cref="IObjectResolver"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to register.
        /// </param>
        void Register(IExecution execution);

        /// <summary>
        /// Adds <see cref="IDisposable"/> to bind.
        /// </summary>
        /// <param name="disposable">
        /// <see cref="IDisposable"/> to register.
        /// </param>
        void Register(IDisposable disposable);
    }
}
