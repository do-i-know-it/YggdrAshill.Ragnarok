using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Defines extensions for <see cref="ITermination"/>.
    /// </summary>
    public static class TerminationExtension
    {
        /// <summary>
        /// Creates <see cref="ISpan"/> from <see cref="ITermination"/> and <see cref="IOrigination"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to terminate in <see cref="ISpan"/>.
        /// </param>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to originate in <see cref="ISpan"/>.
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
        public static ISpan From(this ITermination termination, IOrigination origination)
        {
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return new Span(origination, termination);
        }
    }
}
