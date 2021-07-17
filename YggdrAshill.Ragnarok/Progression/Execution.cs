using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IExecution"/>.
    /// </summary>
    public sealed class Execution :
        IExecution
    {
        /// <summary>
        /// Creates <see cref="Execution"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="Action"/> to execute.
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
        /// <see cref="Execution"/> to execute none.
        /// </summary>
        public static Execution None { get; } = Of(() => { });

        private readonly Action onExecuted;

        private Execution(Action onExecuted)
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
