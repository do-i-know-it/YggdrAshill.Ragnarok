using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IExecution"/>.
    /// </summary>
    public static class ExecutionExtension
    {
        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="ICondition"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="condition">
        /// <see cref="ICondition"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static IExecution When(this IExecution execution, ICondition condition)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return new ExecuteWhenConditionIsSatisfied(condition, execution);
        }
        private sealed class ExecuteWhenConditionIsSatisfied :
            IExecution
        {
            private readonly ICondition condition;

            private readonly IExecution execution;

            internal ExecuteWhenConditionIsSatisfied(ICondition condition, IExecution execution)
            {
                this.condition = condition;

                this.execution = execution;
            }

            /// <inheritdoc/>
            public void Execute()
            {
                if (!condition.IsSatisfied)
                {
                    return;
                }

                execution.Execute();
            }
        }

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
        /// <see cref="IExecution"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
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

            return new AbortWhenExecutionHasErrored(execution, abortion);
        }
        private sealed class AbortWhenExecutionHasErrored :
            IExecution
        {
            private readonly IExecution execution;

            private readonly IAbortion abortion;

            internal AbortWhenExecutionHasErrored(IExecution execution, IAbortion abortion)
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
