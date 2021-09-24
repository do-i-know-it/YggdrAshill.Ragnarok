using YggdrAshill.Ragnarok.Periodization;
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
        /// Executes <see cref="Action"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="Action"/> to <see cref="Terminate"/>.
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
        /// Executes none.
        /// </summary>
        public static Termination None { get; } = new Termination(() => { });

        private readonly Action onTerminated;

        internal Termination(Action onTerminated)
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
