using YggdrAshill.Ragnarok.Progression;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// <see cref="IExecution"/> to execute each of connected <see cref="IExecution"/> simultaneously.
    /// </summary>
    public sealed class CompositeExecution :
        IExecution,
        IDisposable
    {
        private readonly List<IExecution> executionList = new List<IExecution>();

        internal ITermination Bind(IExecution execution)
        {
            if (!executionList.Contains(execution))
            {
                executionList.Add(execution);
            }

            return Termination.Of(() =>
            {
                if (executionList.Contains(execution))
                {
                    executionList.Remove(execution);
                }
            });
        }

        /// <inheritdoc/>
        public void Execute()
        {
            foreach (var execution in executionList)
            {
                execution.Execute();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            executionList.Clear();
        }
    }
}
