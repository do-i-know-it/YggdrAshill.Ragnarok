using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IOrigination"/>.
    /// </summary>
    public sealed class Origination :
        IOrigination
    {
        /// <summary>
        /// Executes <see cref="Action"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="Action"/> to <see cref="Originate"/>.
        /// </param>
        /// <returns>
        /// <see cref="Origination"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        public static Origination Of(Action origination)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return new Origination(origination);
        }

        /// <summary>
        /// Executes none.
        /// </summary>
        public static Origination None { get; } = new Origination(() => { });

        private readonly Action onOriginated;

        internal Origination(Action onOriginated)
        {
            this.onOriginated = onOriginated;
        }

        /// <inheritdoc/>
        public void Originate()
        {
            onOriginated.Invoke();
        }
    }
}
