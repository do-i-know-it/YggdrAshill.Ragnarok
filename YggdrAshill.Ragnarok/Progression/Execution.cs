using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IExecution"/>.
    /// </summary>
    public sealed class Execution :
        IExecution
    {
        private readonly Action onExecuted;

        #region Constructor

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="onExecuted">
        /// <see cref="Action"/> executed when this has executed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="onExecuted"/> is null.
        /// </exception>
        public Execution(Action onExecuted)
        {
            if (onExecuted == null)
            {
                throw new ArgumentNullException(nameof(onExecuted));
            }

            this.onExecuted = onExecuted;
        }

        /// <summary>
        /// Constructs an instance to do nothing when this has executed.
        /// </summary>
        public Execution()
        {
            onExecuted = () =>
            {

            };
        }

        #endregion

        #region IExecution

        /// <inheritdoc/>
        public void Execute()
        {
            onExecuted.Invoke();
        }

        #endregion
    }
}
