using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Defines extensions for <see cref="ICycle"/>.
    /// </summary>
    public static class CycleExtension
    {
        /// <summary>
        /// Binds <see cref="ICycle"/> to <see cref="ISpan"/>.
        /// </summary>
        /// <param name="cycle">
        /// <see cref="ICycle"/> to be bound to <paramref name="span"/>.
        /// </param>
        /// <param name="span">
        /// <see cref="ISpan"/> to bind <paramref name="cycle"/>.
        /// </param>
        /// <returns>
        /// <see cref="ICycle"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="cycle"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="span"/> is null.
        /// </exception>
        public static ICycle In(this ICycle cycle, ISpan span)
        {
            if (cycle is null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return new Cycle(span, cycle);
        }
        private sealed class Cycle :
            IOrigination,
            ITermination,
            IExecution,
            ISpan,
            ICycle
        {
            public IOrigination Origination => this;

            public ITermination Termination => this;

            public IExecution Execution => this;

            public ISpan Span => this;

            private readonly ISpan span;

            private readonly ICycle cycle;
            
            internal Cycle(ISpan span, ICycle cycle)
            {
                this.span = span;

                this.cycle = cycle;
            }

            public void Originate()
            {
                span.Origination.Originate();

                cycle.Span.Origination.Originate();
            }

            public void Terminate()
            {
                cycle.Span.Termination.Terminate();

                span.Termination.Terminate();
            }

            public void Execute()
            {
                cycle.Execution.Execute();
            }
        }

        /// <summary>
        /// Binds <see cref="ICycle"/> to <see cref="IOrigination"/> and <see cref="ITermination"/>.
        /// </summary>
        /// <param name="cycle">
        /// <see cref="ICycle"/> to be bound to <paramref name="origination"/> and <paramref name="termination"/>.
        /// </param>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind to <see cref="ICycle"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind to <see cref="ICycle"/>.
        /// </param>
        /// <returns>
        /// <see cref="ICycle"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="cycle"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static ICycle Between(this ICycle cycle, IOrigination origination, ITermination termination)
        {
            if (cycle is null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return cycle.In(origination.To(termination));
        }

        /// <summary>
        /// Runs <see cref="ICycle"/>, originating, executing and terminating it.
        /// </summary>
        /// <param name="cycle">
        /// <see cref="ICycle"/> to run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="cycle"/> is null.
        /// </exception>
        public static void Run(this ICycle cycle)
        {
            if (cycle is null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }

            using (cycle.Span.Open())
            {
                cycle.Execution.Execute();
            }
        }
    }
}
