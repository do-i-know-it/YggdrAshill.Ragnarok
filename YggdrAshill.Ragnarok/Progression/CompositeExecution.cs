using YggdrAshill.Ragnarok.Progression;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Executes each of bound <see cref="IExecution"/> simultaneously.
    /// </summary>
    public sealed class CompositeExecution :
        IExecution,
        IDisposable
    {
        private readonly List<IExecution> executionList = new List<IExecution>();

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
    }
}
