using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Token to <see cref="Abort(Exception)"/>.
    /// </summary>
    public interface IAbortion
    {
        /// <summary>
        /// Aborts thrown <see cref="Exception"/> .
        /// </summary>
        /// <param name="exception">
        /// <see cref="Exception"/> thrown.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="exception"/> is null.
        /// </exception>
        void Abort(Exception exception);
    }
}
