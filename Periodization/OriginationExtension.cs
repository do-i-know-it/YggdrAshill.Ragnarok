using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Defines extensions for <see cref="IOrigination"/>.
    /// </summary>
    public static class OriginationExtension
    {
        /// <summary>
        /// Creates <see cref="ISpan"/> from <see cref="IOrigination"/> and <see cref="ITermination"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to originate in <see cref="ISpan"/>.
        /// </param>
        /// <param name="termination">
        /// <see cref="ITermination"/> to terminate in <see cref="ISpan"/>.
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
        public static ISpan To(this IOrigination origination, ITermination termination)
        {
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Span(origination, termination);
        }
    }
}
