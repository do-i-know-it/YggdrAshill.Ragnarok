using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Extension for Pefiodization.
    /// </summary>
    public static class PeriodizationExtension
    {
        /// <summary>
        /// Connects <see cref="ITermination"/> with <see cref="CompositeTermination"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="composite">
        /// <see cref="CompositeTermination"/> to bind.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="composite"/> is null.
        /// </exception>
        public static void Bind(this ITermination termination, CompositeTermination composite)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            composite.Bind(termination);
        }
    }
}
