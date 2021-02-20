using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IActivation"/>.
    /// </summary>
    public sealed class Activation :
        IActivation
    {
        private readonly Func<IExecution> onActivated;

        #region Constructor

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="onActivated">
        /// <see cref="Func{IExecution}"/> executed when this has activated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="onActivated"/> is null.
        /// </exception>
        public Activation(Func<IExecution> onActivated)
        {
            if (onActivated == null)
            {
                throw new ArgumentNullException(nameof(onActivated));
            }

            this.onActivated = onActivated;
        }

        /// <summary>
        /// Constructs an instance to do nothing when this has activated.
        /// </summary>
        public Activation()
        {
            onActivated = () =>
            {
                return new Execution();
            };
        }

        #endregion

        #region IActivation

        /// <inheritdoc/>
        public IExecution Activate()
        {
            return onActivated.Invoke();
        }

        #endregion
    }
}
