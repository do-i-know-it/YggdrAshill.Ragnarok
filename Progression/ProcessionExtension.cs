using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IProcession"/>.
    /// </summary>
    public static class ProcessionExtension
    {
        /// <summary>
        /// Binds <see cref="IProcession"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="procession">
        /// <see cref="IProcession"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="IProcession"/> bounded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IProcession Bind(this IProcession procession, IAbortion abortion)
        {
            if (procession == null)
            {
                throw new ArgumentNullException(nameof(procession));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new Procession(procession, abortion);
        }
        private sealed class Procession :
            IProcession
        {
            private readonly IOrigination origination;

            private readonly IExecution execution;

            private readonly ITermination termination;

            internal Procession(IProcession procession, IAbortion abortion)
            {
                origination = ((IOrigination)procession).Bind(abortion);

                execution = ((IExecution)procession).Bind(abortion);

                termination = ((ITermination)procession).Bind(abortion);
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
}
