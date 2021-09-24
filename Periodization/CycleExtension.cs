using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Defines extensions for <see cref="ICycle"/>.
    /// </summary>
    public static class CycleExtension
    {
        /// <summary>
        /// Runs <see cref="ICycle"/>, originating, executing and terminating it.
        /// </summary>
        /// <param name="cycle">
        /// <see cref="ICycle"/> to run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="cycle"/> is null.
        /// </exception>
        public static void Run(this ICycle cycle)
        {
            if (cycle is null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }

            cycle.Originate();

            try
            {
                cycle.Execute();
            }
            finally
            {
                cycle.Terminate();
            }
        }
    }
}
