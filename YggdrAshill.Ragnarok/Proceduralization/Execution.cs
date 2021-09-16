using System;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    /// <summary>
    /// Implementation of <see cref="IExecution"/>.
    /// </summary>
    public sealed class Execution :
        IExecution
    {
        /// <summary>
        /// Executes <see cref="Action"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="Action"/> to <see cref="Execute"/>.
        /// </param>
        /// <returns>
        /// <see cref="Execution"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        public static Execution Of(Action execution)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            return new Execution(execution);
        }

        /// <summary>
        /// Executes none.
        /// </summary>
        public static Execution None { get; } = new Execution(() => { });

        private readonly Action onExecuted;

        internal Execution(Action onExecuted)
        {
            this.onExecuted = onExecuted;
        }

        /// <inheritdoc/>
        public void Execute()
        {
            onExecuted.Invoke();
        }
    }
}
