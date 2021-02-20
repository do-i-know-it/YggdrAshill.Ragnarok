using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implemenation of <see cref="IExecution"/>.
    /// Collects other tokens of <see cref="IExecution"/> to execute when this has executed.
    /// </summary>
    public sealed class CompositeExecution :
        IExecution,
        ITermination
    {
        private readonly List<IExecution> executionList = new List<IExecution>();

        /// <summary>
        /// Binds <see cref="ITermination"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="execution"></param>
        /// <returns>
        /// <see cref="ITermination"/> to disconnect <paramref name="execution"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        public ITermination Bind(IExecution execution)
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

        /// <summary>
        /// Executes each <see cref="IExecution"/> when this has executed.
        /// </summary>
        public void Execute()
        {
            foreach (var execution in executionList)
            {
                execution.Execute();
            }
        }

        /// <summary>
        /// Clear List for <see cref="IExecution"/>.
        /// </summary>
        public void Terminate()
        {
            executionList.Clear();
        }
    }
}
