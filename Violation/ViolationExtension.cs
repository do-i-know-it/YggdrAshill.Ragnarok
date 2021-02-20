using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Violation
{
    /// <summary>
    /// Extension for Violation.
    /// </summary>
    public static class ViolationExtension
    {
        /// <summary>
        /// Converts <see cref="IExecution"/> to abortable <see cref="IExecution"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// Abortable <see cref="IExecution"/> when it has thrown <see cref="Exception"/>.
        /// </returns>
        public static IExecution Bind(this IExecution execution, IAbortion abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new ExecutionWithAbortion(execution, abortion);
        }
    }
}
