using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="ITermination"/>.
    /// </summary>
    public static class TerminationExtension
    {
        /// <summary>
        /// Binds <see cref="ITermination"/> to <see cref="ICondition"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="condition">
        /// <see cref="ICondition"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static ITermination When(this ITermination termination, ICondition condition)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return new TerminateWhenConditionIsSatisfied(condition, termination);
        }
        private sealed class TerminateWhenConditionIsSatisfied :
            ITermination
        {
            private readonly ICondition condition;

            private readonly ITermination termination;

            internal TerminateWhenConditionIsSatisfied(ICondition condition, ITermination termination)
            {
                this.condition = condition;

                this.termination = termination;
            }

            /// <inheritdoc/>
            public void Terminate()
            {
                if (!condition.IsSatisfied)
                {
                    return;
                }

                termination.Terminate();
            }
        }

        /// <summary>
        /// Binds <see cref="ITermination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> bounded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static ITermination Bind(this ITermination termination, IAbortion abortion)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new AbortWhenTerminationHasErrored(termination, abortion);
        }
        private sealed class AbortWhenTerminationHasErrored :
            ITermination
        {
            private readonly ITermination termination;

            private readonly IAbortion abortion;

            internal AbortWhenTerminationHasErrored(ITermination termination, IAbortion abortion)
            {
                this.termination = termination;

                this.abortion = abortion;
            }

            /// <inheritdoc/>
            public void Terminate()
            {
                try
                {
                    termination.Terminate();
                }
                catch (Exception exception)
                {
                    abortion.Abort(exception);
                }
            }
        }
    }
}
