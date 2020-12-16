using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;
using YggdrAshill.Ragnarok.Unification;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class ExecutionList :
        IExecutionList
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
