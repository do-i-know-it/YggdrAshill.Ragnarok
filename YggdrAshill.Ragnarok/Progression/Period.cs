using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Period :
        IPeriod
    {
        /// <summary>
        /// Executes <see cref="IOrigination"/> and <see cref="ITermination"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to originate <see cref="Period"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="ITermination"/> to terminate <see cref="Period"/>.
        /// </param>
        /// <returns>
        /// <see cref="Period"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static Period Of(IOrigination origination, ITermination termination)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Period(origination, termination);
        }

        /// <summary>
        /// Executes <see cref="Action"/> and <see cref="Action"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="Action"/> to originate <see cref="Period"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="Action"/> to terminate <see cref="Period"/>.
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
        /// Executes none.
        /// </summary>
        public static Period None { get; } = new Period(Origination.None, Termination.None);

        private readonly IOrigination origination;

        private readonly ITermination termination;

        private Period(IOrigination origination, ITermination termination)
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
