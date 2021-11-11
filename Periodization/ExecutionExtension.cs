using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Defines extensions for <see cref="IExecution"/>.
    /// </summary>
    public static class ExecutionExtension
    {
        /// <summary>
        /// Creates <see cref="ICycle"/> from <see cref="IExecution"/> and <see cref="ISpan"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to execute in <see cref="ICycle"/>.
        /// </param>
        /// <param name="span">
        /// <see cref="ISpan"/> to originate and terminate in <see cref="ICycle"/>.
        /// </param>
        /// <returns>
        /// <see cref="ICycle"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="span"/> is null.
        /// </exception>
        public static ICycle In(this IExecution execution, ISpan span)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return new Cycle(span, execution);
        }
        private sealed class Cycle :
            ICycle
        {
            public ISpan Span { get; }

            public IExecution Execution { get; }

            internal Cycle(ISpan span, IExecution execution)
            {
                Span = span;

                Execution = execution;
            }
        }

        /// <summary>
        /// Creates <see cref="ICycle"/> from <see cref="IExecution"/>, <see cref="IOrigination"/> and <see cref="ITermination"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to execute in <see cref="ICycle"/>.
        /// </param>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to originate in <see cref="ICycle"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="ITermination"/> to terminate in <see cref="ICycle"/>.
        /// </param>
        /// <returns>
        /// <see cref="ICycle"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static ICycle Between(this IExecution execution, IOrigination origination, ITermination termination)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return execution.In(origination.To(termination));
        }
    }
}
