using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IAbortion"/>.
    /// </summary>
    public sealed class Abortion :
        IAbortion
    {
        /// <summary>
        /// Executes <see cref="Action{T}"/>.
        /// </summary>
        /// <param name="abortion">
        /// <see cref="Action{T}"/> to abort <see cref="Exception"/>.
        /// </param>
        /// <returns>
        /// <see cref="Abortion"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static Abortion Of(Action<Exception> abortion)
        {
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new Abortion(abortion);
        }

        /// <summary>
        /// Executes <see cref="Action"/>.
        /// </summary>
        /// <param name="abortion">
        /// <see cref="Action"/> to abort <see cref="Exception"/>.
        /// </param>
        /// <returns>
        /// <see cref="Abortion"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static Abortion Of(Action abortion)
        {
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new Abortion(exception =>
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                abortion.Invoke();
            });
        }

        /// <summary>
        /// Executes none.
        /// </summary>
        public static Abortion None { get; } = Of(() => { });

        private readonly Action<Exception> onAborted;

        private Abortion(Action<Exception> onAborted)
        {
            this.onAborted = onAborted;
        }

        /// <inheritdoc/>
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            onAborted.Invoke(exception);
        }
    }
}