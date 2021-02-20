using YggdrAshill.Ragnarok.Progression;
using YggdrAshill.Ragnarok.Violation;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Extension for Violation.
    /// </summary>
    public static class ViolationExtension
    {
        /// <summary>
        /// Converts <see cref="IExecution"/> with <see cref="Action{Exception}"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action{Exception}"/> to bind.
        /// </param>
        /// <returns>
        /// Abortable <see cref="IExecution"/> when it has thrown <see cref="Exception"/>.
        /// </returns>
        public static IExecution Bind(this IExecution execution, Action<Exception> abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return execution.Bind(new Abortion(abortion));
        }
    }
}
