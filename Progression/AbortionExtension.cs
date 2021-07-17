using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IAbortion"/>.
    /// </summary>
    public static class AbortionExtension
    {
        #region Originate

        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> bounded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IOrigination Bind(this IOrigination origination, IAbortion abortion)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new Origination(origination, abortion);
        }
        private sealed class Origination :
            IOrigination
        {
            private readonly IOrigination origination;

            private readonly IAbortion abortion;

            internal Origination(IOrigination origination, IAbortion abortion)
            {
                this.origination = origination;

                this.abortion = abortion;
            }

            /// <inheritdoc/>
            public void Originate()
            {
                try
                {
                    origination.Originate();
                }
                catch (Exception exception)
                {
                    abortion.Abort(exception);
                }
            }
        }

        #endregion

        #region Execute

        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> bounded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IExecution Bind(this IExecution execution, IAbortion abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new Execution(execution, abortion);
        }
        private sealed class Execution :
            IExecution
        {
            private readonly IExecution execution;

            private readonly IAbortion abortion;

            internal Execution(IExecution execution, IAbortion abortion)
            {
                this.execution = execution;

                this.abortion = abortion;
            }

            /// <inheritdoc/>
            public void Execute()
            {
                try
                {
                    execution.Execute();
                }
                catch (Exception exception)
                {
                    abortion.Abort(exception);
                }
            }
        }

        #endregion

        #region Terminate

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

        #endregion
    }
}
