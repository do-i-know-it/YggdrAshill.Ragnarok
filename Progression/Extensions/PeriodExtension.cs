using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IPeriod"/>.
    /// </summary>
    public static class PeriodExtension
    {
        /// <summary>
        /// Initializes <see cref="IPeriod"/>.
        /// </summary>
        /// <param name="period">
        /// <see cref="IPeriod"/> to initialize.
        /// </param>
        /// <returns>
        /// <see cref="IDisposable"/> to finalize period.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="period"/> is null.
        /// </exception>
        public static IDisposable Initialize(this IPeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            period.Originate();

            return period.ToDisposable();
        }

        /// <summary>
        /// Transacts <see cref="IExecution"/> in <see cref="IPeriod"/>.
        /// </summary>
        /// <param name="period">
        /// <see cref="IPeriod"/> for transaction scope.
        /// </param>
        /// <param name="execution">
        /// <see cref="IExecution"/> executed in <paramref name="period"/>.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> for transaction.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="period"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        public static IExecution Transact(this IPeriod period, IExecution execution)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            return new Transaction(period, execution);
        }
        private sealed class Transaction :
            IExecution
        {
            private readonly IPeriod period;

            private readonly IExecution execution;

            public Transaction(IPeriod period, IExecution execution)
            {
                this.period = period;

                this.execution = execution;
            }

            /// <inheritdoc/>
            public void Execute()
            {
                using (period.Initialize())
                {
                    execution.Execute();
                }
            }
        }

        /// <summary>
        /// Converts <see cref="IPeriod"/> into <see cref="IOrigination"/>.
        /// </summary>
        /// <param name="period">
        /// <see cref="IPeriod"/> to convert.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> converted.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="period"/> is null.
        /// </exception>
        [Obsolete]
        public static IOrigination Origination(this IPeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            return period;
        }
      
        /// <summary>
        /// Converts <see cref="IPeriod"/> into <see cref="ITermination"/>.
        /// </summary>
        /// <param name="period">
        /// <see cref="IPeriod"/> to convert.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> converted.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="period"/> is null.
        /// </exception>
        [Obsolete]
        public static ITermination Termination(this IPeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            return period;
        }
    }
}
