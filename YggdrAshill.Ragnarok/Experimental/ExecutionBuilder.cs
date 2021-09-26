using YggdrAshill.Ragnarok.Periodization;
using System.Linq;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal static class ExecutionBuilder
    {
        internal static IExecutionBuilder Default { get; } = new None();
        private sealed class None :
            IExecutionBuilder,
            IExecution
        {
            public IExecution Build()
            {
                return this;
            }

            public IExecutionBuilder Configure(IExecution execution)
            {
                return new One(execution);
            }

            public void Execute()
            {

            }
        }

        private sealed class One :
            IExecutionBuilder
        {
            private readonly IExecution first;

            internal One(IExecution first)
            {
                this.first = first;
            }

            public IExecution Build()
            {
                return first;
            }

            public IExecutionBuilder Configure(IExecution execution)
            {
                if (first == execution)
                {
                    return this;
                }

                return new Listed(new[] { first, execution });
            }
        }

        private sealed class Listed :
            IExecutionBuilder
        {
            private readonly IEnumerable<IExecution> executions;

            internal Listed(IEnumerable<IExecution> executions)
            {
                this.executions = executions;
            }

            public IExecution Build()
            {
                return new Execution(executions.ToArray());
            }

            public IExecutionBuilder Configure(IExecution execution)
            {
                if (executions.Contains(execution))
                {
                    return this;
                }

                return new Listed(executions.Append(execution));
            }
        }
        private sealed class Execution :
            IExecution
        {
            private readonly IExecution[] executions;

            internal Execution(IExecution[] executions)
            {
                this.executions = executions;
            }

            public void Execute()
            {
                foreach (var execution in executions)
                {
                    execution.Execute();
                }
            }
        }
    }
}
