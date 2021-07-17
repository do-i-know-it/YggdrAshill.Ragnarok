using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="ITermination"/>.
    /// </summary>
    public static class TerminationExtension
    {
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

            return new Termination(termination, abortion);
        }
        private sealed class Termination :
            ITermination
        {
            private readonly ITermination termination;

            private readonly IAbortion abortion;

            internal Termination(ITermination termination, IAbortion abortion)
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
