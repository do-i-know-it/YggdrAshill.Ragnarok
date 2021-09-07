using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IProcess"/>.
    /// </summary>
    public static class ProcessExtension
    {
        /// <summary>
        /// Converts <see cref="IProcess"/> into <see cref="IExecution"/>.
        /// </summary>
        /// <param name="process">
        /// <see cref="IProcess"/> to convert.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> converted.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="process"/> is null.
        /// </exception>
        [Obsolete]
        public static IExecution Execution(this IProcess process)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            return process;
        }
    }
}
