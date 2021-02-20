using System;

namespace YggdrAshill.Ragnarok.Violation
{
    /// <summary>
    /// Aborts when <see cref="Exception"/> has been thrown.
    /// </summary>
    public interface IAbortion
    {
        /// <summary>
        /// Aborts.
        /// </summary>
        /// <param name="exception">
        /// <see cref="Exception"/> thrown.
        /// </param>
        void Abort(Exception exception);
    }
}
