using YggdrAshill.Ragnarok.Administration;
using System.Collections.Generic;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class ExecutionList :
        IExecution,
        IExecutionCollection
    {
        private readonly List<IExecution> executionList = new List<IExecution>();

        public void Execute()
        {
            foreach (var execution in executionList)
            {
                execution.Execute();
            }
        }

        public void Collect(IExecution execution)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            if (executionList.Contains(execution))
            {
                return;
            }

            executionList.Add(execution);
        }
    }
}
