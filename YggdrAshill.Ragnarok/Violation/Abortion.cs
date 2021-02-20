using YggdrAshill.Ragnarok.Violation;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IAbortion"/>.
    /// </summary>
    public sealed class Abortion :
        IAbortion
    {
        private readonly Action<Exception> onAborted;

        #region Constructor

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="onAborted">
        /// <see cref="Action{Exception}"/> executed when this has aborted.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="onAborted"/> is null.
        /// </exception>
        public Abortion(Action<Exception> onAborted)
        {
            if (onAborted == null)
            {
                throw new ArgumentNullException(nameof(onAborted));
            }

            this.onAborted = onAborted;
        }

        /// <summary>
        /// Constructs an instance to do nothing when this has aborted.
        /// </summary>
        public Abortion()
        {
            onAborted = (_) =>
            {

            };
        }

        #endregion

        #region IAbortion

        /// <inheritdoc/>
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            onAborted.Invoke(exception);
        }

        #endregion
    }
}
