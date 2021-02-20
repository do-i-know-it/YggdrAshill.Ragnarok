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
        private readonly Action onTerminated;

        #region Constructor

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="onTerminated">
        /// <see cref="Action"/> executed when this has teminated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="onTerminated"/> is null.
        /// </exception>
        public Termination(Action onTerminated)
        {
            if (onTerminated == null)
            {
                throw new ArgumentNullException(nameof(onTerminated));
            }

            this.onTerminated = onTerminated;
        }

        /// <summary>
        /// Constructs an instance to do nothing when this has teminated.
        /// </summary>
        public Termination()
        {
            onTerminated = () =>
            {

            };
        }

        #endregion

        #region ITermination

        /// <inheritdoc/>
        public void Terminate()
        {
            onTerminated.Invoke();
        }

        #endregion
    }
}
