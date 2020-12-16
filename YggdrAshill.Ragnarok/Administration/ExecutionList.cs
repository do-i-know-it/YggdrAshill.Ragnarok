using YggdrAshill.Ragnarok.Administration;
using System.Collections.Generic;
using System;
using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    public sealed class ExecutionList :
        IExecution,
        IExecutionCollection,
        ITermination
    {
        private readonly List<IExecution> executionList = new List<IExecution>();

        public void Execute()
        {
            foreach (var execution in executionList)
            {
                execution.Execute();
            }
        }

        public ITermination Collect(IExecution execution)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            if (!executionList.Contains(execution))
            {
                executionList.Add(execution);
            }

            return new Termination(() =>
            {
                if (executionList.Contains(execution))
                {
                    executionList.Remove(execution);
                }
            });
        }

        public void Terminate()
        {
            executionList.Clear();
        }
    }
}
