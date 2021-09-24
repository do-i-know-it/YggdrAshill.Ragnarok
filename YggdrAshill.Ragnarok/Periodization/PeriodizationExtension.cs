using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines extensions for Periodization.
    /// </summary>
    public static class PeriodizationExtension
    {
        /// <summary>
        /// Creates <see cref="ISpan"/> from <see cref="IOrigination"/> and <see cref="Action"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to originate in <see cref="ISpan"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="Action"/> to terminate in <see cref="ISpan"/>.
        /// </param>
        /// <returns>
        /// <see cref="ISpan"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static ISpan To(this IOrigination origination, Action termination)
        {
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return origination.To(new Termination(termination));
        }

        /// <summary>
        /// Creates <see cref="ISpan"/> from <see cref="ITermination"/> and <see cref="Action"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to terminate in <see cref="ISpan"/>.
        /// </param>
        /// <param name="origination">
        /// <see cref="Action"/> to originate in <see cref="ISpan"/>.
        /// </param>
        /// <returns>
        /// <see cref="ISpan"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        public static ISpan From(this ITermination termination, Action origination)
        {
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return termination.From(new Origination(origination));
        }

        /// <summary>
        /// Creates <see cref="ICycle"/> from <see cref="IExecution"/>, two <see cref="Action"/>s.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to execute in <see cref="ICycle"/>.
        /// </param>
        /// <param name="origination">
        /// <see cref="Action"/> to originate in <see cref="ICycle"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="Action"/> to terminate in <see cref="ICycle"/>.
        /// </param>
        /// <returns>
        /// <see cref="ICycle"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static ICycle Between(this IExecution execution, Action origination, Action termination)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return execution.Between(Origination.Of(origination), Termination.Of(termination));
        }
    }
}
