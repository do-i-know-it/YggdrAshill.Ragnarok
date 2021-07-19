using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Period :
        IPeriod
    {
        /// <summary>
        /// Creates <see cref="Period"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="Action"/> to originate.
        /// </param>
        /// <param name="termination">
        /// <see cref="Action"/> to terminate.
        /// </param>
        /// <returns>
        /// <see cref="Period"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static Period Of(Action origination, Action termination)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Period(Origination.Of(origination), Termination.Of(termination));
        }

        /// <summary>
        /// <see cref="Period"/> to execute none.
        /// </summary>
        public static Period None { get; } = new Period(Origination.None, Termination.None);

        private readonly Origination origination;

        private readonly Termination termination;

        private Period(Origination origination, Termination termination)
        {
            this.origination = origination;

            this.termination = termination;
        }

        /// <inheritdoc/>
        public void Originate()
        {
            origination.Originate();
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            termination.Terminate();
        }
    }
}
