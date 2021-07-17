using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IExecution"/>.
    /// </summary>
    public static class ExecutionExtension
    {
        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> bounded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IExecution Bind(this IExecution execution, IAbortion abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new Execution(execution, abortion);
        }
        private sealed class Execution :
            IExecution
        {
            private readonly IExecution execution;

            private readonly IAbortion abortion;

            internal Execution(IExecution execution, IAbortion abortion)
            {
                this.execution = execution;

                this.abortion = abortion;
            }

            /// <inheritdoc/>
            public void Execute()
            {
                try
                {
                    execution.Execute();
                }
                catch (Exception exception)
                {
                    abortion.Abort(exception);
                }
            }
        }
    }
}
