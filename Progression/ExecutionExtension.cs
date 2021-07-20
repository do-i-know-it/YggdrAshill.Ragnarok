using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IExecution"/>.
    /// </summary>
    public static class ExecutionExtension
    {
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

            return new Execution(condition, execution);
        }
        internal sealed class Execution :
            IExecution
        {
            private readonly ICondition condition;

            private readonly IExecution execution;

            internal Execution(ICondition condition, IExecution execution)
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
    }
}
