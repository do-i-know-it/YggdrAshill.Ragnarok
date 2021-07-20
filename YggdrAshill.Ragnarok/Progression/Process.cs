using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Process :
        IProcess
    {
        /// <summary>
        /// Executes <see cref="IOrigination"/>, <see cref="IExecution"/> and <see cref="ITermination"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to originate <see cref="Process"/>.
        /// </param>
        /// <param name="execution">
        /// <see cref="IExecution"/> to execute <see cref="Process"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="ITermination"/> to terminate <see cref="Process"/>.
        /// </param>
        /// <returns>
        /// <see cref="Process"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static Process Of(IOrigination origination, IExecution execution, ITermination termination)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Process(origination, execution, termination);
        }

        /// <summary>
        /// Executes <see cref="Action"/>, <see cref="Action"/> and <see cref="Action"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="Action"/> to originate <see cref="Process"/>.
        /// </param>
        /// <param name="execution">
        /// <see cref="Action"/> to execute <see cref="Process"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="Action"/> to terminate <see cref="Process"/>.
        /// </param>
        /// <returns>
        /// <see cref="Process"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static Process Of(Action origination, Action execution, Action termination)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Process(Origination.Of(origination), Execution.Of(execution), Termination.Of(termination));
        }

        /// <summary>
        /// <see cref="IProcess"/> to execute none.
        /// </summary>
        public static Process None { get; } = new Process(Origination.None, Execution.None, Termination.None);

        private readonly IOrigination origination;

        private readonly IExecution execution;

        private readonly ITermination termination;

        private Process(IOrigination origination, IExecution execution, ITermination termination)
        {
            this.origination = origination;

            this.execution = execution;

            this.termination = termination;
        }

        /// <inheritdoc/>
        public void Originate()
        {
            origination.Originate();
        }

        /// <inheritdoc/>
        public void Execute()
        {
            execution.Execute();
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            termination.Terminate();
        }
    }
}
