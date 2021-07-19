using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IPeriod"/>.
    /// </summary>
    public static class PeriodExtension
    {
        /// <summary>
        /// Converts <see cref="IPeriod"/> into <see cref="IOrigination"/>.
        /// </summary>
        /// <param name="period">
        /// <see cref="IPeriod"/> to convert.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> converted.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="period"/> is null.
        /// </exception>
        public static IOrigination Origination(this IPeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            return period;
        }
      
        /// <summary>
        /// Converts <see cref="IPeriod"/> into <see cref="ITermination"/>.
        /// </summary>
        /// <param name="period">
        /// <see cref="IPeriod"/> to convert.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> converted.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="period"/> is null.
        /// </exception>
        public static ITermination Termination(this IPeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            return period;
        }
    }
}
