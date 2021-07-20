using YggdrAshill.Ragnarok.Progression;
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
        /// <see cref="Action"/> to originate.
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
        public static Origination None { get; } = Of(() => { });

        private readonly Action onOriginated;

        private Origination(Action onOriginated)
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
