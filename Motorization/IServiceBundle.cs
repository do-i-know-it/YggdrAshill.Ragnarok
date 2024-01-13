using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Defines service lifecycle for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// Type of service lifecycle.
    /// </typeparam>
    public interface IServiceBundle<out T>
        where T : notnull
    {
        /// <summary>
        /// <typeparamref name="T"/>s for service lifecycle.
        /// </summary>
        IReadOnlyList<T> Package { get; }
    }
}
