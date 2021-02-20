using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Extension for Pefiodization.
    /// </summary>
    public static class ProgressionExtension
    {
        /// <summary>
        /// Connects <see cref="IExecution"/> with <see cref="CompositeExecution"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="composite">
        /// <see cref="CompositeExecution"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> to disconnect <paramref name="execution"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="composite"/> is null.
        /// </exception>
        public static ITermination Bind(this IExecution execution, CompositeExecution composite)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            return composite.Bind(execution);
        }
    }
}
