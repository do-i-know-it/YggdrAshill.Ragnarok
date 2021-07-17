using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="ITermination"/>.
    /// </summary>
    public sealed class Termination :
        ITermination
    {
        /// <summary>
        /// Creates <see cref="Termination"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="Action"/> to terminate.
        /// </param>
        /// <returns>
        /// <see cref="Termination"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static Termination Of(Action termination)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Termination(termination);
        }

        /// <summary>
        /// <see cref="Termination"/> to terminate none.
        /// </summary>
        public static Termination None { get; } = Of(() => { });

        private readonly Action onTerminated;

        private Termination(Action onTerminated)
        {
            this.onTerminated = onTerminated;
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            onTerminated.Invoke();
        }
    }
}
